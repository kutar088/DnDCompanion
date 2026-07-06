<template>
  <div class="container my-4">
    <h2 class="dnd-title mb-4">
      <i class="fa-solid fa-shield-halved"></i> Класи D&amp;D 5e
    </h2>

    <div v-if="loading" class="text-center py-5">
      <div class="spinner-border" style="color: var(--dnd-red);"></div>
    </div>

    <div v-else class="row g-4">
      <div
        v-for="cls in classes"
        :key="cls.id"
        class="col-12 col-md-6 col-lg-4"
        data-aos="fade-up"
      >
        <div class="card h-100">
          <div class="card-body">
            <h4 class="text-info">{{ cls.name }}</h4>
            <p class="mb-2">
              <span class="badge" style="background-color: var(--dnd-red);">
                <i class="fa-solid fa-dice-d6"></i> Hit Die: {{ cls.hitDie }}
              </span>
            </p>
            <p><strong>Основна характеристика:</strong> {{ cls.primaryAbility }}</p>
            <p><strong>Saving Throws:</strong> {{ cls.savingThrows }}</p>
            <p class="text-muted small">{{ cls.description }}</p>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import { http } from '@/interceptors/http.js'

export default {
  name: 'ClassesView',
  data() { return { classes: [], loading: true } },
  async mounted() {
    try {
      const { data } = await http.get('/classes')
      this.classes = data
    } finally {
      this.loading = false
    }
  }
}
</script>
