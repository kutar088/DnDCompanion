<template>
  <nav class="navbar navbar-expand-lg navbar-dark bg-dark shadow-sm">
    <div class="container">
      <router-link to="/" class="navbar-brand fw-bold d-flex align-items-center">
        <i class="fa-solid fa-dice-d20 me-2" style="color: var(--dnd-red);"></i>
        <span class="dnd-title">DnDCompanion</span>
      </router-link>

      <button
        class="navbar-toggler"
        type="button"
        @click="mobileOpen = !mobileOpen"
      >
        <span class="navbar-toggler-icon"></span>
      </button>

      <div class="collapse navbar-collapse" :class="{ show: mobileOpen }">
        <ul class="navbar-nav ms-auto align-items-lg-center">
          <li class="nav-item">
            <router-link class="nav-link" to="/" @click="mobileOpen = false">
              <i class="fa-solid fa-house"></i> Головна
            </router-link>
          </li>
          <li class="nav-item">
            <router-link class="nav-link" to="/races" @click="mobileOpen = false">
              <i class="fa-solid fa-users"></i> Раси
            </router-link>
          </li>
          <li class="nav-item">
            <router-link class="nav-link" to="/classes" @click="mobileOpen = false">
              <i class="fa-solid fa-shield-halved"></i> Класи
            </router-link>
          </li>
          <li class="nav-item">
            <router-link class="nav-link" to="/spells" @click="mobileOpen = false">
              <i class="fa-solid fa-wand-sparkles"></i> Заклинання
            </router-link>
          </li>
          <li class="nav-item">
            <router-link class="nav-link" to="/about" @click="mobileOpen = false">
              <i class="fa-solid fa-circle-info"></i> Про нас
            </router-link>
          </li>
          <li class="nav-item" v-if="auth.isAuthenticated">
            <router-link class="nav-link" to="/characters" @click="mobileOpen = false">
              <i class="fa-solid fa-id-badge"></i> Мої персонажі
            </router-link>
          </li>
          <li class="nav-item" v-if="!auth.isAuthenticated">
            <router-link class="nav-link" to="/login" @click="mobileOpen = false">
              <i class="fa-solid fa-right-to-bracket"></i> Вхід
            </router-link>
          </li>

<li class="nav-item user-dropdown" v-else ref="dropdownEl">
            <button
              class="nav-link user-dropdown-toggle"
              type="button"
              @click.stop="userMenuOpen = !userMenuOpen"
            >
              <img
                v-if="auth.user?.avatarSeed"
                :src="avatarUrl(auth.user.avatarSeed)"
                alt="avatar"
                class="rounded-circle me-2 avatar-sm"
                width="28"
                height="28"
              />
              <i v-else class="fa-solid fa-user-circle me-1"></i>
              {{ auth.user?.name || auth.user?.email || 'Профіль' }}
              <i class="fa-solid fa-caret-down ms-1"></i>
            </button>

            <ul v-if="userMenuOpen" class="user-menu">
              <li>
                <router-link class="user-menu-item" to="/profile" @click="closeAll">
                  <i class="fa-solid fa-user-pen"></i> Профіль
                </router-link>
              </li>
              <li class="user-menu-divider"></li>
              <li>
                <button class="user-menu-item" @click="doLogout">
                  <i class="fa-solid fa-right-from-bracket"></i> Вийти
                </button>
              </li>
            </ul>
          </li>
        </ul>
      </div>
    </div>
  </nav>
</template>

<script>
import { useAuthStore } from '@/stores/auth'
import { avatarUrl } from '@/utils/avatar.js'

export default {
  name: 'HeaderComponent',
  data() {
    return {
      auth: useAuthStore(),
      userMenuOpen: false,
      mobileOpen: false
    }
  },
  mounted() {
    document.addEventListener('click', this.onOutsideClick)
  },
  beforeUnmount() {
    document.removeEventListener('click', this.onOutsideClick)
  },
  methods: {
    avatarUrl,
    onOutsideClick(event) {
      if (this.$refs.dropdownEl && !this.$refs.dropdownEl.contains(event.target)) {
        this.userMenuOpen = false
      }
    },
    closeAll() {
      this.userMenuOpen = false
      this.mobileOpen = false
    },
    doLogout() {
      this.closeAll()
      this.auth.logout()
      this.$router.push('/')
    }
  }
}
</script>

<style scoped>
.user-dropdown {
  position: relative;
}

.user-dropdown-toggle {
  background: transparent;
  border: none;
  color: rgba(255, 255, 255, 0.75);
  cursor: pointer;
  display: flex;
  align-items: center;
  padding: 0.5rem 1rem;
}

.user-dropdown-toggle:hover {
  color: white;
}

.user-menu {
  position: absolute;
  top: 100%;
  right: 0;
  min-width: 180px;
  background: #2b3035;
  border: 1px solid rgba(255, 255, 255, 0.15);
  border-radius: 6px;
  box-shadow: 0 6px 20px rgba(0, 0, 0, 0.5);
  list-style: none;
  margin: 0.25rem 0 0 0;
  padding: 0.5rem 0;
  z-index: 1050;
}

.user-menu-item {
  display: block;
  width: 100%;
  text-align: left;
  padding: 0.5rem 1rem;
  color: rgba(255, 255, 255, 0.85);
  background: transparent;
  border: none;
  cursor: pointer;
  text-decoration: none;
  font-size: 0.95rem;
}

.user-menu-item:hover {
  background: rgba(200, 30, 30, 0.15);
  color: white;
}

.user-menu-item i {
  width: 20px;
  margin-right: 6px;
}

.user-menu-divider {
  height: 1px;
  background: rgba(255, 255, 255, 0.1);
  margin: 0.25rem 0;
}

.avatar-sm {
  background: #fff;
  object-fit: cover;
}

@media (max-width: 991px) {
  .user-menu {
    position: static;
    box-shadow: none;
    border: none;
    background: transparent;
    margin-top: 0;
  }
}
</style>
