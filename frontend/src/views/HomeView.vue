<template>
  <div class="container my-4">
    <HeroComponent />
    <FeaturesComponent />
    <StatsComponent />
    <HowItWorksComponent />

    <section class="text-center my-5 py-4" data-aos="fade-up">
      <h2 class="dnd-title mb-3">Готовий вирушити в пригоду?</h2>
      <p class="text-muted mb-4">
        Приєднуйся до тисяч інших мандрівників.
      </p>
      <router-link :to="ctaTo" class="btn btn-dnd btn-lg">
        <i class="fa-solid fa-dice-d20"></i> {{ ctaLabel }}
      </router-link>
    </section>
  </div>
</template>

<script>
import HeroComponent from '@/components/HeroComponent.vue'
import FeaturesComponent from '@/components/FeaturesComponent.vue'
import StatsComponent from '@/components/StatsComponent.vue'
import HowItWorksComponent from '@/components/HowItWorksComponent.vue'
import { useAuthStore } from '@/stores/auth'

export default {
  name: 'HomeView',
  components: { HeroComponent, FeaturesComponent, StatsComponent, HowItWorksComponent },
  data() {
    return { auth: useAuthStore() }
  },
  computed: {
    ctaTo() { return this.auth.isAuthenticated ? '/characters' : '/register' },
    ctaLabel() { return this.auth.isAuthenticated ? 'До моїх персонажів' : 'Зареєструватись' }
  }
}
</script>
