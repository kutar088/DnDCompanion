<template>
  <section class="hero-bg p-5 mb-4 rounded-3 text-center" data-aos="fade-up">
    <div class="container py-4">
      <h1 class="display-3 fw-bold dnd-title">
        <i class="fa-solid fa-dice-d20" style="color: var(--dnd-red);"></i>
        DnDCompanion
      </h1>
      <p class="lead mt-3">
        Твій компаньйон для Dungeons &amp; Dragons 5e.<br />
        Створюй персонажів, вивчай раси та класи, тримай усе під рукою.
      </p>
      <div class="d-flex gap-3 justify-content-center mt-4 flex-wrap">
        <router-link :to="primaryCtaTo" class="btn btn-dnd btn-lg">
          <i class="fa-solid fa-user-plus"></i>
          {{ primaryCtaLabel }}
        </router-link>
        <router-link to="/races" class="btn btn-outline-light btn-lg">
          <i class="fa-solid fa-users"></i> Огляд рас
        </router-link>
      </div>
    </div>
  </section>
</template>

<script>
import { useAuthStore } from '@/stores/auth'

export default {
  name: 'HeroComponent',
  data() {
    return { auth: useAuthStore() }
  },
  computed: {
    primaryCtaTo() {
      return this.auth.isAuthenticated ? '/characters' : '/login'
    },
    primaryCtaLabel() {
      return this.auth.isAuthenticated ? 'До персонажів' : 'Почати'
    }
  }
}
</script>
