# Fitness Center Management System

Система управления фитнес-центром с разделением на клиентскую, тренерскую и административную роли. Реализована на .NET 8 с использованием Clean Architecture, CQRS и Domain-Driven Design.

## Стек технологий

- **.NET 8** — платформа
- **ASP.NET Core Identity** — аутентификация и управление пользователями
- **Entity Framework Core 8** — ORM
- **SQLite** — база данных 
- **MediatR** — реализация CQRS
- **FluentValidation** — валидация запросов
- **AutoMapper** — маппинг объектов
- **JWT Bearer Authentication** — API-аутентификация
- **Docker / Docker Compose** — контейнеризация

## Архитектура

Проект построен на принципах Clean Architecture и Domain-Driven Design с разделением на четыре слоя:

| Слой | Назначение |
|------|------------|
| **Core** | Доменная модель: сущности, перечисления, исключения, стратегии, фабрики |
| **Application** | Сценарии использования: CQRS (команды и запросы), валидаторы, маппинг, Result-паттерн |
| **Infrastructure** | Реализация доступа к данным: EF Core, репозитории, Unit of Work|
| **API** | Презентационный слой: REST API (JWT) и Web-интерфейс (Razor Pages) |

```
API (Controllers) → Application (MediatR) → Infrastructure (Repositories) → Core (Domain)
```

Ключевые паттерны:
- **Repository + Unit of Work** — абстракция доступа к данным
- **CQRS + MediatR** — разделение команд и запросов
- **Strategy** — поведение абонементов (лимитированные / безлимитные, заморозка)
- **Factory** — создание абонементов
- **Result** — явная обработка ошибок
- **Pipeline Behaviors** — автоматическая валидация

## Структура репозитория

```
fitness_center.backend/
├── Core/                     # Доменный слой
│   ├── Abstractions/         # Интерфейсы репозиториев
│   ├── Entities/             # Сущности (Client, Trainer, Workout, Membership и др.)
│   ├── Enums/                # BookingStatus, WorkoutStatus
│   ├── Exceptions/           # DomainException, WorkoutException, MembershipException
│   ├── Factories/            # MembershipFactory
│   └── Strategies/           # IVisitStrategy, IFreezeStrategy
│
├── Application/              # Слой приложения
│   ├── Common/               # Behaviors, Models, Mappings
│   └── Features/             # Модули (Bookings, Clients, Trainers, Workouts и др.)
│
├── Infrastructure/           # Инфраструктурный слой
│   ├── Data/                 # AppDbContext, DbInitializer
│   └── Repositories/         # EfRepository, EfUnitOfWork
│
├── API/                      # REST API (JWT, Swagger, Controllers)      
│
├── Dockerfile                # Основной образ приложения
├── Dockerfile.migrations     # Образ для миграций
├── docker-compose.yml              
└── entrypoint.sh           
```

## Запуск

### Требования

- .NET 8 SDK
- Docker(для контейнеризации)

### Локальный запуск (без Docker)

```bash
# Восстановление зависимостей
dotnet restore

# Сборка
dotnet build

# Применение миграций (SQLite)
dotnet ef migrations add InitialCreate --project Infrastructure --startup-project API
dotnet ef database update --project Infrastructure --startup-project API

# Запуск API
cd API
dotnet run
```

После запуска:
- API Swagger: `https://localhost:7034/swagger`
- Web-интерфейс: `https://localhost:7100`

### Запуск через Docker

```bash
# Сборка образов
docker-compose build

# Запуск контейнеров в фоновом режиме
docker-compose up -d

# Просмотр логов
docker-compose logs -f api

# Остановка
docker-compose down
```

После запуска:
- API: `https://localhost:5001/swagger`
- Web-интерфейс: `https://localhost:6001` (если включён в docker-compose.yml)

### Миграции базы данных

При локальном запуске миграции применяются вручную командой `dotnet ef database update`.

При запуске через Docker миграции применяются автоматически через `entrypoint.sh` при старте контейнера. Скрипт выполняет до пяти попыток в случае ошибки.

Для ручного применения миграций в Docker:

```bash
docker-compose run --rm migrations
```

## Переменные окружения

Настройки из `appsettings.json` переопределяются переменными окружения в Docker. Синтаксис: двойное подчёркивание `__` заменяет двоеточие `:` в JSON-пути.

| Переменная | Назначение |
|------------|------------|
| `AuthMode` | Режим работы: `Api` (Swagger + JWT) или `Web` (Razor Pages) |
| `Jwt__Key` | Секретный ключ для подписи JWT (минимум 32 символа) |
| `Jwt__Issuer` | Издатель токена |
| `Jwt__Audience` | Аудитория токена |
| `Jwt__ExpiryMinutes` | Время жизни токена (минуты) |
| `AdminSettings__Email` | Email администратора |
| `AdminSettings__Password` | Пароль администратора |
| `ConnectionStrings__SqliteConnection` | Строка подключения к SQLite |

Пример переопределения в `docker-compose.yml`:

```yaml
environment:
  - AuthMode=Api
  - Jwt__Key=your-secret-key-here
  - AdminSettings__Password=Admin123!
  - ConnectionStrings__SqliteConnection=Data Source=/app/data/fitness.db
```

## Аутентификация и роли

Система поддерживает два режима аутентификации:

| Режим | Механизм | Применение |
|-------|----------|------------|
| `Api` | JWT Bearer Token | REST API, Swagger |
| `Web` | Cookie (ASP.NET Core Identity) | Razor Pages |

Роли:
- **Admin** — полный доступ ко всем операциям
- **Client** — управление профилем, запись на тренировки, просмотр абонемента
- **Trainer** — управление профилем, просмотр расписания

Политики авторизации:
- `ClientOwnerPolicy` — доступ к ресурсам только своего профиля
- `TrainerOwnerPolicy` — доступ к ресурсам только своего профиля
- `BookingOwnerPolicy` — доступ к бронированию только его владельца

## API Endpoints

Основные эндпоинты (полный список доступен в Swagger):

| Метод | URL | Описание | Доступ |
|-------|-----|----------|--------|
| POST | `/api/auth/register` | Регистрация клиента | Публичный |
| POST | `/api/auth/login` | Вход (JWT) | Публичный |
| GET | `/api/clients/profile` | Профиль клиента | Client |
| GET | `/api/trainers/schedule` | Расписание тренера | Trainer |
| GET | `/api/workouts` | Список тренировок | Публичный |
| POST | `/api/workouts/{id}/bookings` | Запись на тренировку | Client |
| GET | `/api/workouts/{id}/bookings` | Список записей | Admin, Trainer |


## Планируемые улучшения
- Refresh Token — продление сессии без повторного ввода пароля
- Подтверждение email — верификация пользователей через email-ссылку
- Redis-кеширование — кеширование частых запросов (список тренировок, типы абонементов)
- Integration Tests — тестирование сценариев использования на реальной БД
- Пагинация и фильтрация — единый подход к пагинации для всех списков
- Сортировка — возможность сортировки по нескольким полям


