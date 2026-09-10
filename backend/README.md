# Kovan

Kovan, .NET 8 ve Temiz Mimari (Clean Architecture) prensipleriyle geliştirilmiş modern, çok kiracılı (multi-tenant) bir ERP benzeri uygulamadır. Ürün, müşteri, fatura, envanter ve daha fazlasını yönetmek için sağlam bir arka uç altyapısı sunar.

## ✨ Özellikler

- **Çoklu Kiracı (Multi-Tenancy):** Veriler kiracı bazında ayrılmıştır.
- **Kimlik Doğrulama ve Yetkilendirme:** JWT tabanlı güvenli kimlik doğrulama ve rol bazlı erişim kontrolü.
- **Kullanıcı Yönetimi:** E-posta yoluyla kullanıcı kaydı ve davet sistemi.
- **Ürün ve Envanter Yönetimi:** Ürünleri, depoları yönetin ve envanter hareketleriyle stok seviyelerini takip edin.
- **Satış ve Faturalandırma:** Müşteri faturaları oluşturun, ödemeleri takip edin ve PDF formatında fatura çıktısı alın.
- **Satın Alma Yönetimi:** Tedarikçileri ve satın alma siparişlerini yönetin.
- **PDF Oluşturma:** Dinamik olarak PDF faturalar ve barkodlu ürün etiketleri oluşturun.
- **Arka Plan Servisleri:** Vadesi geçmiş faturaların durumunu otomatik olarak güncelleyin.

## 🛠️ Teknoloji Mimarisi

- **.NET 8**
- **ASP.NET Core:** Web API
- **Entity Framework Core:** ORM
- **PostgreSQL:** Veritabanı
- **MediatR:** CQRS deseni
- **FluentValidation:** İstek (request) validasyonu
- **Serilog:** Yapısal (structured) loglama
- **QuestPDF:** PDF oluşturma
- **ZXing.Net:** Barkod oluşturma

## 🚀 Başlarken

Projeyi yerel makinenizde ayağa kaldırmak için aşağıdaki adımları izleyin.

### Gereksinimler

- .NET 8 SDK
- PostgreSQL

### Kurulum ve Yapılandırma

1.  **Hassas Bilgileri Yapılandırın (User Secrets)**
    `appsettings.json` dosyalarında tutulmayan veritabanı bağlantısı ve JWT anahtarı gibi hassas bilgileri yapılandırın. Projenin kök dizininde aşağıdaki komutları çalıştırın:

    ```bash
    dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Host=localhost;Database=KovanDb;Username=postgres;Password=your-password" --project src/Presentation/Kovan.Api
    dotnet user-secrets set "Jwt:Key" "use-a-random-secret-at-least-32-characters-long" --project src/Presentation/Kovan.Api
    ```

2.  **Veritabanı Migration'larını Uygulayın**
    Otomatik migration varsayılan olarak kapalıdır. Değişiklikleri veritabanına manuel olarak uygulayın:

    ```bash
    dotnet ef database update --project src/Infrastructure/Kovan.Infrastructure --startup-project src/Presentation/Kovan.Api
    ```

3.  **Uygulamayı Çalıştırın**
    ```bash
    dotnet run --project src/Presentation/Kovan.Api
    ```
