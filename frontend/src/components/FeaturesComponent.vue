<template>
  <section class="row g-4" data-aos="fade-up" data-aos-delay="150">
    <div v-for="f in features" :key="f.to" class="col-12 col-md-6 col-lg-3">
      <router-link :to="f.to" class="text-decoration-none">
        <div class="card h-100 text-center feature-card">
          <div class="card-body">
            <i :class="f.icon" class="display-4" :style="{ color: f.color }"></i>
            <h4 class="mt-3 text-white">{{ f.title }}</h4>
            <p class="text-muted small mb-0">{{ f.description }}</p>
          </div>
        </div>
      </router-link>
    </div>
  </section>
</template>

<script>
export default {
  name: 'FeaturesComponent',
  data() {
    return {
      features: [
        {
          to: '/characters',
          icon: 'fa-solid fa-id-badge',
          color: 'var(--dnd-red)',
          title: 'Персонажі',
          description: 'Створюй карти персонажів з характеристиками та біографією.'
        },
        {
          to: '/races',
          icon: 'fa-solid fa-users text-warning',
          color: '',
          title: 'Раси',
          description: 'Довідник рас D&D 5e з бонусами й рисами.'
        },
        {
          to: '/classes',
          icon: 'fa-solid fa-shield-halved text-info',
          color: '',
          title: 'Класи',
          description: "12 класів D&D 5e, кубик здоров'я, характеристики."
        },
        {
          to: '/spells',
          icon: 'fa-solid fa-wand-sparkles text-success',
          color: '',
          title: 'Заклинання',
          description: 'Каталог заклинань, згрупований за рівнями та школами.'
        }
      ]
    }
  }
}
</script>

<style scoped>
.feature-card {
  transition: transform 0.15s ease, box-shadow 0.15s ease, border-color 0.15s ease;
  cursor: pointer;
  border: 1px solid rgba(255, 255, 255, 0.08);
}
.feature-card:hover {
  transform: translateY(-4px);
  box-shadow: 0 8px 20px rgba(0, 0, 0, 0.4);
  border-color: var(--dnd-red);
}
</style>
