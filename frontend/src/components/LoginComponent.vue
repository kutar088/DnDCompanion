<template>
  <div class="card shadow" data-aos="fade-up">
    <div class="card-body p-4">
      <h3 class="mb-3 text-center dnd-title">Вхід</h3>

      <div v-if="error" class="alert alert-danger">{{ error }}</div>

      <form @submit.prevent="submit">
        <div class="mb-3">
          <label for="email" class="form-label">Email</label>
          <input
            id="email"
            v-model="form.email"
            type="email"
            class="form-control"
            required
          />
        </div>
        <div class="mb-3">
          <label for="password" class="form-label">Пароль</label>
          <input
            id="password"
            v-model="form.password"
            type="password"
            class="form-control"
            minlength="6"
            required
          />
        </div>

        <button type="submit" class="btn btn-dnd w-100" :disabled="loading">
          {{ loading ? 'Зачекайте...' : 'Увійти' }}
        </button>
      </form>

      <div class="text-center mt-3">
        <router-link to="/register" class="btn btn-link p-0">
          Немає акаунта? Зареєструватись
        </router-link>
      </div>
    </div>
  </div>
</template>

<script>
import { useAuthStore } from '@/stores/auth'
import { getHttpErrorMessage } from '@/interceptors/http.js'

export default {
  name: 'LoginComponent',
  data() {
    return {
      auth: useAuthStore(),
      form: { email: '', password: '' },
      loading: false,
      error: ''
    }
  },
  methods: {
    async submit() {
      this.error = ''
      this.loading = true
      try {
        await this.auth.login({ email: this.form.email, password: this.form.password })
        const redirect = this.$route.query.redirect || '/characters'
        this.$router.push(redirect)
      } catch (e) {
        this.error =
          e.response?.status === 401
            ? 'Невірний email або пароль.'
            : getHttpErrorMessage(e, 'Помилка входу.')
      } finally {
        this.loading = false
      }
    }
  }
}
</script>
