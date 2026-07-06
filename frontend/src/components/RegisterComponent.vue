<template>
  <div class="card shadow" data-aos="fade-up">
    <div class="card-body p-4">
      <h3 class="mb-3 text-center dnd-title">Реєстрація</h3>

      <div v-if="error" class="alert alert-danger">{{ error }}</div>

      <form @submit.prevent="submit">
        <div class="mb-3">
          <label class="form-label">Ім'я</label>
          <input v-model="form.name" type="text" class="form-control" required />
        </div>
        <div class="mb-3">
          <label class="form-label">Email</label>
          <input v-model="form.email" type="email" class="form-control" required />
        </div>
        <div class="mb-3">
          <label class="form-label">Пароль</label>
          <input
            v-model="form.password"
            type="password"
            class="form-control"
            minlength="6"
            required
          />
        </div>

        <button type="submit" class="btn btn-dnd w-100" :disabled="loading">
          {{ loading ? 'Зачекайте...' : 'Зареєструватись' }}
        </button>
      </form>

      <div class="text-center mt-3">
        <router-link to="/login" class="btn btn-link p-0">
          Вже є акаунт? Увійти
        </router-link>
      </div>
    </div>
  </div>
</template>

<script>
import { useAuthStore } from '@/stores/auth'
import { getHttpErrorMessage } from '@/interceptors/http.js'

export default {
  name: 'RegisterComponent',
  data() {
    return {
      auth: useAuthStore(),
      form: { name: '', email: '', password: '' },
      loading: false,
      error: ''
    }
  },
  methods: {
    async submit() {
      this.error = ''
      this.loading = true
      try {
        await this.auth.register(this.form)
        this.$router.push('/characters')
      } catch (e) {
        this.error =
          e.response?.status === 409
            ? 'Користувач з таким email вже існує.'
            : getHttpErrorMessage(e, 'Помилка реєстрації.')
      } finally {
        this.loading = false
      }
    }
  }
}
</script>
