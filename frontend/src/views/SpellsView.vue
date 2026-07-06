<template>
  <div class="container my-4">
    <div class="d-flex justify-content-between align-items-center mb-4 flex-wrap gap-3">
      <h2 class="dnd-title mb-0">
        <i class="fa-solid fa-wand-sparkles"></i> Заклинання D&amp;D 5e
      </h2>

      <div class="d-flex gap-2 flex-wrap">
        <button
          class="btn btn-sm"
          :class="filterLevel === null ? 'btn-dnd' : 'btn-outline-light'"
          @click="filterLevel = null"
        >
          Усі
        </button>
        <button
          v-for="l in availableLevels"
          :key="l"
          class="btn btn-sm"
          :class="filterLevel === l ? 'btn-dnd' : 'btn-outline-light'"
          @click="filterLevel = l"
        >
          {{ l === 0 ? 'Замовляння' : `${l} рівень` }}
        </button>
      </div>
    </div>

    <div v-if="loading" class="text-center py-5">
      <div class="spinner-border" style="color: var(--dnd-red);"></div>
    </div>

    <div v-else class="row g-4">
      <div
        v-for="spell in filteredSpells"
        :key="spell.id"
        class="col-12 col-md-6 col-lg-4"
        data-aos="fade-up"
      >
        <div class="card h-100 spell-card">
          <div class="card-body">
            <div class="d-flex justify-content-between align-items-start">
              <h5 class="dnd-title mb-1" style="color: var(--dnd-red);">
                <i :class="schoolIcon(spell.school)"></i>
                {{ spell.name }}
              </h5>
              <span class="badge bg-secondary">
                {{ spell.level === 0 ? 'Замовляння' : `${spell.level} рівень` }}
              </span>
            </div>
            <p class="mb-2 text-muted small">{{ spell.school }}</p>

            <ul class="list-unstyled small mb-3">
              <li>
                <i class="fa-solid fa-clock text-info"></i>
                <strong> Час:</strong> {{ spell.castingTime }}
              </li>
              <li>
                <i class="fa-solid fa-bullseye text-warning"></i>
                <strong> Дальність:</strong> {{ spell.range }}
              </li>
              <li>
                <i class="fa-solid fa-flask text-success"></i>
                <strong> Компоненти:</strong> {{ spell.components }}
              </li>
              <li>
                <i class="fa-solid fa-hourglass-half text-danger"></i>
                <strong> Тривалість:</strong> {{ spell.duration }}
              </li>
            </ul>

            <p class="small">{{ spell.description }}</p>

            <p class="mt-3 mb-0">
              <small class="text-muted">
                <i class="fa-solid fa-users"></i> {{ spell.classes }}
              </small>
            </p>
          </div>
        </div>
      </div>
    </div>

    <div v-if="!loading && filteredSpells.length === 0" class="text-center py-5">
      <i class="fa-solid fa-magnifying-glass display-1 text-muted"></i>
      <p class="text-muted mt-3">Немає заклинань обраного рівня.</p>
    </div>
  </div>
</template>

<script>
import { http } from '@/interceptors/http.js'

const SCHOOL_ICONS = {
  'Викликання': 'fa-solid fa-fire-flame-curved',
  'Прикликання': 'fa-solid fa-hat-wizard',
  'Захисна': 'fa-solid fa-shield',
  'Ілюзія': 'fa-solid fa-eye-slash',
  'Ворожіння': 'fa-solid fa-eye',
  'Зачарування': 'fa-solid fa-heart',
  'Некромантія': 'fa-solid fa-skull',
  'Перетворення': 'fa-solid fa-arrows-rotate'
}

export default {
  name: 'SpellsView',
  data() {
    return {
      spells: [],
      loading: true,
      filterLevel: null
    }
  },
  computed: {
    availableLevels() {
      const set = new Set(this.spells.map((s) => s.level))
      return [...set].sort((a, b) => a - b)
    },
    filteredSpells() {
      if (this.filterLevel === null) return this.spells
      return this.spells.filter((s) => s.level === this.filterLevel)
    }
  },
  async mounted() {
    try {
      const { data } = await http.get('/spells')
      this.spells = data
    } finally {
      this.loading = false
    }
  },
  methods: {
    schoolIcon(school) {
      return SCHOOL_ICONS[school] || 'fa-solid fa-wand-sparkles'
    }
  }
}
</script>

<style scoped>
.spell-card {
  border-left: 3px solid var(--dnd-red);
}
</style>
