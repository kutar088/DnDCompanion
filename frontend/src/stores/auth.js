import { defineStore } from 'pinia'
import { http } from '@/interceptors/http.js'

const TOKEN_KEY = 'dnd_access_token'

export const useAuthStore = defineStore('auth', {
  state: () => ({
    token: localStorage.getItem(TOKEN_KEY) || '',
    user: null,
    initialized: false
  }),

  getters: {
    isAuthenticated: (state) => Boolean(state.token)
  },

  actions: {
    setToken(token) {
      this.token = token || ''
      if (this.token) {
        localStorage.setItem(TOKEN_KEY, this.token)
      } else {
        localStorage.removeItem(TOKEN_KEY)
      }
    },

    async initialize() {
      if (!this.token) {
        this.initialized = true
        return
      }
      try {
        await this.fetchMe()
      } catch {
        this.logout()
      } finally {
        this.initialized = true
      }
    },

    async register({ name, email, password }) {
      await http.post('/auth/register', { name, email, password })
      return this.login({ email, password })
    },

    async login({ email, password }) {
      const { data } = await http.post('/auth/login', { email, password })
      this.setToken(data.access_token)
      await this.fetchMe()
    },

    async fetchMe() {
      const { data } = await http.get('/auth/me')
      this.user = data
      return data
    },

    async updateProfile({ name, avatarSeed }) {
      const { data } = await http.put('/auth/profile', { name, avatarSeed })
      this.user = data
      return data
    },

    async changePassword({ oldPassword, newPassword }) {
      await http.post('/auth/change-password', { oldPassword, newPassword })
    },

    logout() {
      this.setToken('')
      this.user = null
    }
  }
})
