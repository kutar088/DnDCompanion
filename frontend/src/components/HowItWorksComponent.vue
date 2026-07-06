<template>
  <section class="my-5" data-aos="fade-up">
    <h2 class="text-center dnd-title mb-5">Як це працює</h2>
    <div class="row g-4">
      <div class="col-12 col-md-4 text-center">
        <div class="step-circle">1</div>
        <h5 class="mt-3">Зареєструйся</h5>
        <p class="text-muted small">
          Створи безкоштовний акаунт за декілька секунд.
          Твої персонажі будуть прив'язані до нього.
        </p>
      </div>
      <div class="col-12 col-md-4 text-center">
        <div class="step-circle">2</div>
        <h5 class="mt-3">Створи персонажа</h5>
        <p class="text-muted small">
          Обери расу, клас, задай ім'я, характеристики та біографію.
          Все зберігається на сервері.
        </p>
      </div>
      <div class="col-12 col-md-4 text-center">
        <div class="step-circle">3</div>
        <h5 class="mt-3">Грай</h5>
        <p class="text-muted small">
          Тримай усі дані під рукою, кидай кубики через вбудований роллер,
          звіряйся з довідниками.
        </p>
      </div>
    </div>
  </section>
</template>

<script>
export default { name: 'HowItWorksComponent' }
</script>

<style scoped>
.step-circle {
  width: 80px;
  height: 80px;
  border-radius: 50%;
  background-color: var(--dnd-red);
  color: white;
  font-size: 2rem;
  font-weight: bold;
  font-family: 'Georgia', serif;
  display: flex;
  align-items: center;
  justify-content: center;
  margin: 0 auto;
  box-shadow: 0 4px 15px rgba(200, 30, 30, 0.4);
}
</style>
