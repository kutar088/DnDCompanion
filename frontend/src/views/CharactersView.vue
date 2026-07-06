<template>
  <div class="container my-4">
    <div class="d-flex justify-content-between align-items-center mb-4">
      <h2 class="dnd-title mb-0">
        <i class="fa-solid fa-id-badge"></i> Мої персонажі
      </h2>
      <router-link to="/characters/new" class="btn btn-dnd">
        <i class="fa-solid fa-plus"></i> Новий персонаж
      </router-link>
    </div>

    <div v-if="error" class="alert alert-danger">{{ error }}</div>

    <div v-if="loading" class="text-center py-5">
      <div class="spinner-border" style="color: var(--dnd-red);"></div>
    </div>

    <div v-else-if="characters.length === 0" class="text-center py-5">
      <i class="fa-solid fa-inbox display-1 text-muted"></i>
      <p class="text-muted mt-3">
        У тебе поки немає персонажів. Створи першого!
      </p>
      <router-link to="/characters/new" class="btn btn-dnd mt-2">
        <i class="fa-solid fa-plus"></i> Створити персонажа
      </router-link>
    </div>

    <div v-else class="row g-4">
      <div
        v-for="char in characters"
        :key="char.id"
        class="col-12 col-md-6 col-lg-4"
        data-aos="fade-up"
      >
        <CharacterCardComponent
          :character="char"
          @edit="editCharacter"
          @delete="deleteCharacter"
        />
      </div>
    </div>
  </div>
</template>

<script>
import { http, getHttpErrorMessage } from '@/interceptors/http.js'
import CharacterCardComponent from '@/components/CharacterCardComponent.vue'

export default {
  name: 'CharactersView',
  components: { CharacterCardComponent },
  data() {
    return {
      characters: [],
      loading: false,
      error: ''
    }
  },
  async mounted() {
    await this.loadCharacters()
  },
  methods: {
    async loadCharacters() {
      this.loading = true
      this.error = ''
      try {
        const { data } = await http.get('/characters')
        this.characters = Array.isArray(data) ? data : []
      } catch (e) {
        this.error = getHttpErrorMessage(e, 'Не вдалося завантажити персонажів.')
      } finally {
        this.loading = false
      }
    },

    editCharacter(character) {
      this.$router.push(`/characters/${character.id}/edit`)
    },

    async deleteCharacter(id) {
      if (!confirm('Видалити персонажа?')) return
      try {
        await http.delete(`/characters/${id}`)
        await this.loadCharacters()
      } catch (e) {
        alert(getHttpErrorMessage(e, 'Помилка видалення персонажа.'))
      }
    }
  }
}
</script>
