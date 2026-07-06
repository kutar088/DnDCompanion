<template>
  <div class="card h-100 character-card">
    <div class="card-body">
      <div class="d-flex justify-content-between align-items-start">
        <div>
          <h4 class="mb-1 dnd-title">{{ character.name }}</h4>
          <small class="text-muted">
            {{ character.race }} <span class="mx-1">·</span> {{ character.class }}
          </small>
        </div>
        <span class="badge fs-6" style="background-color: var(--dnd-red);">
          Ур. {{ character.level }}
        </span>
      </div>

      <hr />

      <div class="row g-2 mb-2">
        <div class="col-6">
          <i class="fa-solid fa-heart" style="color: var(--dnd-red);"></i>
          <strong> HP:</strong> {{ character.hitPoints }}
        </div>
        <div class="col-6">
          <i class="fa-solid fa-shield" style="color: #6ea8fe;"></i>
          <strong> КЗ:</strong> {{ character.armorClass || 10 }}
        </div>
      </div>

      <div class="row row-cols-3 g-2 text-center small">
        <div v-for="s in stats" :key="s.label">
          <span class="badge bg-secondary w-100 stat-badge">
            {{ s.label }} {{ character[s.field] }}
            <small v-if="mod(character[s.field]) !== null" class="text-white-50">
              ({{ formatMod(mod(character[s.field])) }})
            </small>
          </span>
        </div>
      </div>

      <p v-if="character.description" class="mt-3 text-muted small">
        {{ character.description }}
      </p>
    </div>

    <div class="card-footer bg-transparent border-0">
      <button
        class="btn btn-sm btn-outline-warning me-2"
        @click="$emit('edit', character)"
      >
        <i class="fa-solid fa-scroll"></i> Відкрити лист
      </button>
      <button
        class="btn btn-sm btn-outline-danger"
        @click="$emit('delete', character.id)"
      >
        <i class="fa-solid fa-trash"></i> Видалити
      </button>
    </div>
  </div>
</template>

<script>
export default {
  name: 'CharacterCardComponent',
  props: {
    character: { type: Object, required: true }
  },
  emits: ['edit', 'delete'],
  data() {
    return {
      stats: [
        { label: 'STR', field: 'strength' },
        { label: 'DEX', field: 'dexterity' },
        { label: 'CON', field: 'constitution' },
        { label: 'INT', field: 'intelligence' },
        { label: 'WIS', field: 'wisdom' },
        { label: 'CHA', field: 'charisma' }
      ]
    }
  },
  methods: {
    mod(score) {
      if (typeof score !== 'number') return null
      return Math.floor((score - 10) / 2)
    },
    formatMod(m) {
      return m >= 0 ? `+${m}` : `${m}`
    }
  }
}
</script>
