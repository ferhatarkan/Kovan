# Kovan Frontend

Kovan ERP uygulamasının Angular 20 tabanlı web istemcisi. Backend ile **Clean Architecture** prensipleriyle uyumlu olacak şekilde tasarlanmıştır.

## 🏗️ Mimari

```
src/app/
├── core/                          # Çekirdek katman (framework bağımsız iş kuralları)
│   ├── domain/                    # Domain katmanı (en içte, hiçbir şeye bağımlı değil)
│   │   ├── models/                # Backend DTO'larının karşılığı (Auth, Product, Invoice...)
│   │   ├── repositories/          # Port'lar (abstract class) — implementasyon sözleşmeleri
│   │   └── usecases/              # Uygulama iş kuralları (LoginUseCase, GetProductsUseCase...)
│   ├── infrastructure/            # Adaptör katmanı — port'ları gerçekleştirir
│   │   ├── api/                   # ApiBase + ProblemDetails → ApiError dönüşümü
│   │   ├── interceptors/          # JWT ekleme + refresh token akışı
│   │   ├── repositories/          # REST API implementasyonları (*ApiRepository)
│   │   └── services/              # TokenStorageService (localStorage)
│   └── core.module.ts             # DI kayıtları (port → adapter bağlama)
├── features/                      # Özellik modülleri (lazy-loadable)
│   ├── auth/                      # Login sayfası + AuthService (oturum state'i)
│   ├── products/                  # Ürün listesi (sayfalı)
│   ├── customers/                 # Müşteri listesi (sayfalı)
│   └── invoices/                  # Fatura listesi (durum rozetleri ile)
└── presentation/                  # Sunum kabuğu
    ├── guards/                    # authGuard (kimlik doğrulama)
    └── layout/                    # Sidebar + router-outlet
```

### Backend ile Birebir Uyum

| Backend (Kovan.Api) | Frontend (Angular) |
|---|---|
| `PaginatedList<T>` | `PaginatedList<T>` interface |
| `LoginResponseDto` | `AuthResponse` interface |
| `InvoiceStatus` enum | `InvoiceStatus` enum (aynı değerler) |
| `CustomerType` enum | `CustomerType` enum (aynı değerler) |
| `ProblemDetails` hata yanıtı | `ApiError` sınıfı |
| `DependencyInjection.cs` | `provideCore()` |
| MediatR Commands/Queries | UseCase sınıfları |

### Bağımlılık Kuralı

```
presentation → features → core/domain ← core/infrastructure
```

- **Domain**, hiçbir dış modüle import yapmaz (saf TypeScript).
- **Infrastructure**, yalnızca domain'e bağımlıdır (port'ları implemente eder).
- **Features**, use case'leri çağırır; HTTP istemcisi bilmez.
- API değiştiğinde yalnızca `infrastructure/repositories` güncellenir.

## 🚀 Çalıştırma

```bash
npm install
npm start          # http://localhost:4200 (proxy → http://localhost:5194)
```

Backend'in `http://localhost:5194` üzerinde çalıştığından emin olun:

```bash
cd ../backend
dotnet run --project src/Presentation/Kovan.Api
```

## 📦 Build

```bash
npm run build      # dist/kovan-frontend