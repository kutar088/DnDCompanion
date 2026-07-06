import { createApp } from 'vue'
import { createPinia } from 'pinia'

import App from './App.vue'
import router from './router'
import { useAuthStore } from '@/stores/auth'
import { setAuthTokenGetter, setUnauthorizedHandler } from '@/interceptors/http.js'
import AOS from 'aos'

import 'bootswatch/dist/darkly/bootstrap.min.css'
import 'bootstrap/dist/js/bootstrap.bundle.min.js'
import '@fortawesome/fontawesome-free/css/all.min.css'
import 'aos/dist/aos.css'
import '@/assets/main.css'

const app = createApp(App)
const pinia = createPinia()

app.use(pinia)
app.use(router)
app.use(AOS)

const authStore = useAuthStore(pinia)

setAuthTokenGetter(() => authStore.token)
setUnauthorizedHandler(() => {
  authStore.logout()
  router.push('/login')
})

authStore.initialize().finally(() => {
  AOS.init({ duration: 800, once: true })
  app.mount('#app')
})
