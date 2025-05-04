## BgituSec ASP.NET Core Web API

Этот репозиторий содержит реализацию Web API на ASP.NET Core, переписанную с Java Spring. Оригинальный проект на Java Spring доступен по ссылке: [deskMonitor](https://github.com/Damsmh/deskMonitor).

Проект выполнен по принципам Clean Architecture и CQRS с использованием MediatR, AutoMapper, FluentValidation и поддержкой Server-Sent Events (SSE) для уведомлений в реальном времени.

---

## 📁 Структура проекта

```bash
BgituSec.Solution
├── BgituSec.Domain            
├── BgituSec.Application       
├── BgituSec.Infrastructure    
└── BgituSec.Api                                  
```

---

## ⚙️ Быстрый старт

1. Клонируйте репозиторий:

   ```bash
   git clone https://github.com/Damsmh/BgituSec.git
   ```
2. Настройте строку подключения в `appsettings.json`:

   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Host=localhost;Database=dbname;Username=username;Password=password"
     }
   }
   ```
3. Примените миграции и запустите базу:

   ```bash
   cd BgituSec.Infrastructure
   dotnet ef database update
   ```
4. Запустите API:

   ```bash
   cd BgituSec.Api
   dotnet run
   ```
5. Откройте Swagger: `https://localhost:7110/swagger`

---

## 🛠️ Технологии

* .NET 9 / C#
* ASP.NET Core Web API
* Entity Framework Core + Npgsql
* MediatR
* AutoMapper
* FluentValidation
* Swagger / Swashbuckle

---

© 2025 ВДК
