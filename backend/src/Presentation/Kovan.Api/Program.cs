using Kovan.Api.Middleware;
using Kovan.Api.Services;
using Kovan.Application;
using Kovan.Application.Common.Interfaces;
using Kovan.Domain.Entities;
using Kovan.Domain.Authorization;
using Kovan.Infrastructure;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Text;
using Asp.Versioning;
using AspNetCoreRateLimit;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

var jwtKey = builder.Configuration["Jwt:Key"];
var jwtIssuer = builder.Configuration["Jwt:Issuer"];
var jwtAudience = builder.Configuration["Jwt:Audience"];

if (string.IsNullOrWhiteSpace(jwtKey) || string.IsNullOrWhiteSpace(jwtIssuer) || string.IsNullOrWhiteSpace(jwtAudience))
    throw new InvalidOperationException("Jwt:Key, Jwt:Issuer ve Jwt:Audience yapılandırılmalıdır.");

// Serilog yapılandırmasını ekle
builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration));


// --- Servisleri Konteynera Ekleme ---

// Katmanlara ait servis kayıtlarını çağır
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);


// API projesine özel servisler
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddControllers();
// CORS Yapılandırması
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });

    options.AddPolicy("AllowConfiguredOrigins", policy =>
    {
        var allowedOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() ?? Array.Empty<string>();

        if (allowedOrigins.Any())
        {
            policy.WithOrigins(allowedOrigins)
                  .AllowAnyMethod()
                  .AllowAnyHeader()
                  .AllowCredentials();
        }
        else
        {
            // Fallback to allowing all if not configured
            policy.AllowAnyOrigin()
                  .AllowAnyMethod()
                  .AllowAnyHeader();
        }
    });
});

// Health Checks Yapılandırması
builder.Services.AddHealthChecks();

// Rate Limiting Yapılandırması
builder.Services.AddMemoryCache();
builder.Services.Configure<IpRateLimitOptions>(builder.Configuration.GetSection("IpRateLimiting"));
builder.Services.AddInMemoryRateLimiting();
builder.Services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();

// Caching Yapılandırması (Redis)
var redisEnabled = builder.Configuration.GetValue<bool>("Redis:Enabled");
var redisConnectionString = builder.Configuration.GetValue<string>("Redis:ConnectionString");

if (redisEnabled && !string.IsNullOrEmpty(redisConnectionString))
{
    builder.Services.AddStackExchangeRedisCache(options =>
    {
        options.Configuration = redisConnectionString;
    });
}
else
{
    // Fallback to in-memory cache if Redis is not configured
    builder.Services.AddDistributedMemoryCache();
}

// API Versioning Yapılandırması
builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.ReportApiVersions = true;
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ApiVersionReader = ApiVersionReader.Combine(
        new UrlSegmentApiVersionReader(),
        new HeaderApiVersionReader("x-api-version")
    );
});

// Swagger/OpenAPI yapılandırması
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Kovan API", Version = "v1" });

    // JWT Bearer şemasını tanımla
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header. Example: \"Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    // Tanımlanan şemayı tüm endpoint'lere uygula
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
            },
            new string[] {}
        }
    });
});

// --- ASP.NET Core Identity Servislerini Ekleme ---
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<Kovan.Infrastructure.Persistence.ApplicationDbContext>()
    .AddDefaultTokenProviders();
builder.Services.AddScoped<SignInManager<ApplicationUser>>();


// --- JWT Kimlik Doğrulama Servisini Ekleme ---
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtIssuer,
        ValidAudience = jwtAudience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
    };
});

// --- Politika Tabanlı Yetkilendirme (Policy-Based Authorization) ---
builder.Services.AddAuthorization(options =>
{
    // Fatura oluşturma politikası
    options.AddPolicy("CanCreateInvoices", policy =>
        policy.RequireAuthenticatedUser()
              .RequireClaim("permission", Permissions.Invoices.Create));
});

var app = builder.Build();

// --- Veritabanı Başlangıç İşlemleri (Migration ve Tohumlama) ---
// Bu blok, app.Run() çağrılmadan ÖNCE çalışmalıdır.
if (builder.Configuration.GetValue<bool>("Database:ApplyMigrationsOnStartup"))
{
    using var scope = app.Services.CreateScope();
    var services = scope.ServiceProvider;
    var logger = services.GetRequiredService<ILogger<Program>>();

    try
    {
        // 1. Veritabanı Migration'larını Uygula
        logger.LogInformation("Veritabanı migration'ları uygulanıyor...");
        var context = services.GetRequiredService<Kovan.Infrastructure.Persistence.ApplicationDbContext>();
        if ((await context.Database.GetPendingMigrationsAsync()).Any())
        {
            await context.Database.MigrateAsync();
            logger.LogInformation("Veritabanı migration'ları başarıyla uygulandı.");
        }
        else
        {
            logger.LogInformation("Bekleyen veritabanı migration'ı bulunmuyor.");
        }

        // 2. Veritabanını Tohumla (Seed)
        logger.LogInformation("Veritabanı tohumlama işlemi başlatılıyor...");
        var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
        await Kovan.Infrastructure.Persistence.ApplicationDbContextSeed.SeedDefaultUserAsync(userManager, roleManager);
        logger.LogInformation("Veritabanı tohumlama işlemi başarıyla tamamlandı.");
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "Veritabanı başlangıç işlemleri sırasında bir hata oluştu.");
        // Hata durumunda uygulamanın başlamasını engellemek için fırlatmak önemlidir.
        throw;
    }
}

// Global Hata Yönetimi Middleware'ini pipeline'a ekle.
app.UseMiddleware<ErrorHandlingMiddleware>();

// --- HTTP İstek Pipeline'ını Yapılandırma ---
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Gelen HTTP isteklerini otomatik olarak loglamak için Serilog middleware'ini ekle.
app.UseSerilogRequestLogging();

// Rate Limiting middleware'ini ekle (CORS'un sonrasına ekle)
app.UseIpRateLimiting();

// CORS middleware'ini ekle (Authentication'dan önce olmalı)
var corsPolicy = app.Environment.IsDevelopment() ? "AllowAllOrigins" : "AllowConfiguredOrigins";
app.UseCors(corsPolicy);

// wwwroot klasöründeki statik dosyaların sunulmasını sağlar.
app.UseStaticFiles();

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

// Health Checks endpoint'i
app.MapHealthChecks("/health");
app.MapHealthChecks("/health/detailed", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
{
    ResponseWriter = async (context, report) =>
    {
        context.Response.ContentType = "application/json";
        var response = new
        {
            status = report.Status.ToString(),
            checks = report.Entries.Select(x => new
            {
                name = x.Key,
                status = x.Value.Status.ToString(),
                description = x.Value.Description,
                duration = x.Value.Duration
            })
        };
        await context.Response.WriteAsJsonAsync(response);
    }
});

app.MapControllers();
app.Run();
