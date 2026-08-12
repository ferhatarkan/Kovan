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
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructure(builder.Configuration);


// API projesine özel servisler
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddControllers();
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

// wwwroot klasöründeki statik dosyaların sunulmasını sağlar.
app.UseStaticFiles();

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();
