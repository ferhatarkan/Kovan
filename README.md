# Kovan

## Local configuration

Secrets are not stored in `appsettings*.json`. Configure them with User Secrets before running the API:

```bash
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Host=localhost;Database=KovanDb;Username=postgres;Password=your-password" --project src/Presentation/Kovan.Api
dotnet user-secrets set "Jwt:Key" "use-a-random-secret-at-least-32-characters-long" --project src/Presentation/Kovan.Api
dotnet user-secrets set "Jwt:Issuer" "https://localhost:7001" --project src/Presentation/Kovan.Api
dotnet user-secrets set "Jwt:Audience" "https://localhost:7001" --project src/Presentation/Kovan.Api
```

Apply migrations explicitly; automatic migration is disabled by default:

```bash
dotnet ef database update --project src/Infrastructure/Kovan.Infrastructure --startup-project src/Presentation/Kovan.Api
```
