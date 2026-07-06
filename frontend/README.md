# DnDCompanion — Front-End

Vue 3 + Vite клієнтська частина застосунку DnDCompanion.

## Запуск

```bash
npm install
npm run dev
```

Vite підніме сервер на **http://localhost:5173**. Запити `/auth`, `/characters`,
`/races`, `/classes` автоматично проксуються на бекенд `http://localhost:5014`
(див. `vite.config.js`), тому CORS налаштовувати не потрібно у dev-режимі.

**Обов'язково перед запуском фронта запусти бекенд** (`dotnet run` у папці
`../backend`).

## Скрипти

| Команда | Дія |
|---------|-----|
| `npm run dev` | Vite dev-сервер із hot-reload |
| `npm run build` | Продакшн-збірка (у `dist/`) |
| `npm run preview` | Локальний перегляд продакшн-збірки |

## Структура

```
frontend/
├── index.html
├── vite.config.js
├── package.json
├── jsconfig.json
└── src/
    ├── main.js                    # точка входу
    ├── App.vue                    # корінь SPA
    ├── assets/
    │   └── main.css               # глобальні стилі + D&D-палітра
    ├── components/
    │   ├── HeaderComponent.vue    # навбар
    │   ├── FooterComponent.vue
    │   ├── HeroComponent.vue      # hero-секція
    │   ├── FeaturesComponent.vue  # фічі на головній
    │   ├── LoginComponent.vue     # форма входу
    │   ├── RegisterComponent.vue  # форма реєстрації
    │   ├── CharacterCardComponent.vue    # картка одного персонажа
    │   └── CharacterFormComponent.vue    # модалка створення/редагування
    ├── interceptors/
    │   └── http.js                # axios-екземпляр з JWT-перехоплювачем
    ├── router/
    │   └── index.js               # Vue Router + route guards
    ├── stores/
    │   └── auth.js                # Pinia store (token, user, isAuthenticated)
    └── views/
        ├── HomeView.vue
        ├── LoginView.vue
        ├── RegisterView.vue
        ├── CharactersView.vue     # головний CRUD-екран
        ├── RacesView.vue
        ├── ClassesView.vue
        └── AboutView.vue
```
