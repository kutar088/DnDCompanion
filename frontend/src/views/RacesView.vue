<template>
  <div class="container my-4">
    <h2 class="dnd-title mb-4">
      <i class="fa-solid fa-users"></i> Раси D&amp;D 5e
    </h2>

    <div v-if="loading" class="text-center py-5">
      <div class="spinner-border" style="color: var(--dnd-red);"></div>
    </div>

    <div v-else class="row g-4">
      <div
        v-for="race in races"
        :key="race.id"
        class="col-12 col-md-6 col-lg-4"
        data-aos="fade-up"
      >
        <div class="card h-100">
          <div class="card-body">
            <h4 style="color: var(--dnd-red);">{{ race.name }}</h4>
            <p class="mb-2">
              <span class="badge bg-secondary">{{ race.size }}</span>
              <span class="badge bg-info ms-1">
                <i class="fa-solid fa-bolt"></i> {{ race.speed }} футів
              </span>
            </p>
            <p><strong>Бонуси:</strong> {{ race.abilityBonuses }}</p>
            <p><strong>Риси:</strong> {{ race.traits }}</p>
            <p class="text-muted small">{{ race.description }}</p>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import { http } from '@/interceptors/http.js'

export default {
  name: 'RacesView',
  data() { return { races: [], loading: true } },
  async mounted() {
    try {
      const { data } = await http.get('/races')
      this.races = data
    } finally {
      this.loading = false
    }
  }
}
</script>
