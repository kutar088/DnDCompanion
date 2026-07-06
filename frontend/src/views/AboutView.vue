<template>
  <div class="container my-5">
    <div class="row justify-content-center">
      <div class="col-12 col-lg-8" data-aos="fade-up">
        <div class="card">
          <div class="card-body p-4 p-md-5">
            <h1 class="dnd-title mb-4">
              <i class="fa-solid fa-circle-info" style="color: var(--dnd-red);"></i>
              Про проєкт
            </h1>

            <p class="lead">
              <strong>DnDCompanion</strong> — навчальний Full-Stack MVP-проєкт,
              створений у рамках навчальної практики з конструювання програмного
              забезпечення.
            </p>

            <p>
              Застосунок дозволяє гравцям у Dungeons &amp; Dragons 5e створювати
              картки персонажів, зберігати їхні характеристики, а також
              переглядати довідники по расам і класам.
            </p>

            <h4 class="mt-4 dnd-title">Технологічний стек</h4>
            <div class="row g-3 mt-2">
              <div class="col-md-6">
                <div class="border rounded p-3 h-100">
                  <h5 class="text-info mb-3">
                    <i class="fa-solid fa-server"></i> Back-End
                  </h5>
                  <ul class="mb-0 small">
                    <li>ASP.NET Core 10 (Minimal API)</li>
                    <li>Entity Framework Core + SQLite</li>
                    <li>JWT Bearer Authentication</li>
                    <li>BCrypt.Net (хешування паролів)</li>
                    <li>Swagger / OpenAPI</li>
                  </ul>
                </div>
              </div>
              <div class="col-md-6">
                <div class="border rounded p-3 h-100">
                  <h5 class="text-warning mb-3">
                    <i class="fa-solid fa-code"></i> Front-End
                  </h5>
                  <ul class="mb-0 small">
                    <li>Vue.js 3 + Vite</li>
                    <li>Vue Router</li>
                    <li>Pinia (стан авторизації)</li>
                    <li>Axios (з HTTP-перехоплювачем)</li>
                    <li>Bootstrap 5 + Bootswatch (Darkly)</li>
                    <li>Font Awesome, AOS-анімації</li>
                  </ul>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
export default { name: 'AboutView' }
</script>
