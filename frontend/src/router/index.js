import { createRouter, createWebHistory } from 'vue-router'
import { useAuthStore } from '@/stores/auth'

import HomeView from '@/views/HomeView.vue'
import LoginView from '@/views/LoginView.vue'
import RegisterView from '@/views/RegisterView.vue'
import CharactersView from '@/views/CharactersView.vue'
import CharacterSheetView from '@/views/CharacterSheetView.vue'
import RacesView from '@/views/RacesView.vue'
import ClassesView from '@/views/ClassesView.vue'
import SpellsView from '@/views/SpellsView.vue'
import ProfileView from '@/views/ProfileView.vue'
import AboutView from '@/views/AboutView.vue'

const router = createRouter({
  history: createWebHistory(),
  routes: [
    { path: '/', name: 'home', component: HomeView },
    { path: '/login', name: 'login', component: LoginView, meta: { guestOnly: true } },
    { path: '/register', name: 'register', component: RegisterView, meta: { guestOnly: true } },
    { path: '/characters', name: 'characters', component: CharactersView, meta: { requiresAuth: true } },
    { path: '/characters/new', name: 'character-new', component: CharacterSheetView, meta: { requiresAuth: true } },
    { path: '/characters/:id/edit', name: 'character-edit', component: CharacterSheetView, meta: { requiresAuth: true } },
    { path: '/profile', name: 'profile', component: ProfileView, meta: { requiresAuth: true } },
    { path: '/races', name: 'races', component: RacesView },
    { path: '/classes', name: 'classes', component: ClassesView },
    { path: '/spells', name: 'spells', component: SpellsView },
    { path: '/about', name: 'about', component: AboutView },
    { path: '/:pathMatch(.*)*', redirect: '/' }
  ],
  scrollBehavior() {
    return { top: 0 }
  }
})

router.beforeEach((to) => {
  const auth = useAuthStore()

  if (to.meta.requiresAuth && !auth.isAuthenticated) {
    return { name: 'login', query: { redirect: to.fullPath } }
  }

  if (to.meta.guestOnly && auth.isAuthenticated) {
    return { name: 'characters' }
  }
})

export default router
