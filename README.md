# DnDCompanion

Full-Stack MVP-застосунок для навчальної практики з конструювання програмного
забезпечення. Компаньйон для Dungeons & Dragons 5e: створення персонажів,
перегляд рас і класів.

Проєкт складається з **двох частин**:

- `backend/` — REST API на ASP.NET Core 10 (Minimal API)
- `frontend/` — SPA на Vue 3 + Vite

## Технологічний стек

**Back-End**
- ASP.NET Core 10 (Minimal API)
- Entity Framework Core 10 + SQLite
- JWT Bearer Authentication
- BCrypt.Net-Next — хешування паролів
- Swashbuckle.AspNetCore — Swagger / OpenAPI
- xUnit — юніт-тести (`backend.Tests`)

**Front-End**
- Vue.js 3 + Vite
- Vue Router (SPA-навігація)
- Pinia (стан авторизації)
- Axios (з HTTP-перехоплювачем для JWT)
- Bootstrap 5 + Bootswatch Darkly
- Font Awesome, AOS-анімації

## Структура проєкту

```
DnDCompanion/
├── backend/                     # ASP.NET Core проєкт
│   ├── Data/AppDbContext.cs     # EF Core контекст із сідінгом рас/класів
│   ├── DTOs/Dtos.cs
│   ├── Middleware/ExceptionHandlingMiddleware.cs
│   ├── Models/                  # User, Character, DndRace, DndClass
│   ├── Services/CharacterService.cs   # бізнес-логіка (тестується юніт-тестами)
│   ├── Program.cs               # усі ендпоінти + JWT + Swagger + CORS
│   ├── appsettings.json
│   └── backend.csproj
├── backend.Tests/               # xUnit тестовий проєкт (СР2)
│   └── CharacterServiceTests.cs
├── frontend/                    # Vue 3 + Vite SPA
│   ├── src/
│   │   ├── main.js
│   │   ├── App.vue
│   │   ├── assets/main.css
│   │   ├── components/          # Header, Footer, Hero, Login, Register, Character*
│   │   ├── interceptors/http.js # axios instance + JWT interceptor
│   │   ├── router/index.js      # Vue Router
│   │   ├── stores/auth.js       # Pinia store
│   │   └── views/               # HomeView, LoginView, CharactersView, ...
│   ├── index.html
│   ├── vite.config.js
│   └── package.json
├── DnDCompanion.sln
├── .gitignore
└── README.md
```

## Ендпоінти REST API

| Метод  | URL                    | Опис                                      | Захист  |
|--------|------------------------|-------------------------------------------|---------|
| GET    | `/`                    | Привітальний ендпоінт                     | —       |
| POST   | `/auth/register`       | Реєстрація нового користувача             | —       |
| POST   | `/auth/login`          | Вхід, повертає JWT-токен                  | —       |
| GET    | `/auth/me`             | Дані поточного користувача                | JWT     |
| GET    | `/characters`          | Список персонажів поточного користувача   | JWT     |
| GET    | `/characters/{id}`     | Один персонаж за ID                       | JWT     |
| POST   | `/characters`          | Створити персонажа                        | JWT     |
| PUT    | `/characters/{id}`     | Оновити персонажа                         | JWT     |
| DELETE | `/characters/{id}`     | Видалити персонажа                        | JWT     |
| GET    | `/races`               | Список рас D&D 5e                         | —       |
| GET    | `/races/{id}`          | Одна раса за ID                           | —       |
| GET    | `/classes`             | Список класів D&D 5e                      | —       |
| GET    | `/classes/{id}`        | Один клас за ID                           | —       |
| GET    | `/boom`                | Тестовий ендпоінт (перевірка Middleware)  | —       |

## Запуск

### 1. Передумови
- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- [Node.js 18+](https://nodejs.org/) (з npm)
- `dotnet ef` tool: `dotnet tool install --global dotnet-ef`
  (або `dotnet tool update --global dotnet-ef` якщо вже встановлено під старішу версію)

### 2. Back-End (термінал 1)

```powershell
cd backend
dotnet ef migrations add InitialCreate    # для звіта Т5 (створює папку Migrations/)
dotnet run --launch-profile http
```

- Swagger UI: **http://localhost:5014/swagger**

### 3. Front-End (термінал 2, паралельно)

```powershell
cd frontend
npm install
npm run dev
```

- Веб-застосунок: **http://localhost:5173/**

Vite автоматично проксує запити `/auth`, `/characters`, `/races`, `/classes`
на бекенд, тому CORS у dev-режимі налаштовувати не потрібно.

### 4. Юніт-тести

```powershell
cd backend.Tests
dotnet test
```

## Швидка перевірка вручну

1. Відкрий http://localhost:5173/
2. Натисни «Почати» → **Реєстрація** → введи ім'я, email, пароль
3. Автоматично перекине на «Мої персонажі» — натисни **Новий персонаж**
4. Створи Aragorn'а (Human Ranger, рівень 5, HP 40) → **Створити**
5. Спробуй **Змінити** та **Видалити**
6. Перейди на «Раси» та «Класи» — переконайся, що дані з бази підтягнулись
