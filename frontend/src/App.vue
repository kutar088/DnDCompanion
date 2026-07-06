<template>
  <div class="app-shell d-flex flex-column min-vh-100">
    <HeaderComponent />

    <main class="flex-grow-1">
      <router-view v-slot="{ Component }">
        <transition name="fade" mode="out-in">
          <component :is="Component" />
        </transition>
      </router-view>
    </main>

    <FooterComponent />

    <DiceRollerComponent />
  </div>
</template>

<script>
import HeaderComponent from '@/components/HeaderComponent.vue'
import FooterComponent from '@/components/FooterComponent.vue'
import DiceRollerComponent from '@/components/DiceRollerComponent.vue'

export default {
  name: 'App',
  components: { HeaderComponent, FooterComponent, DiceRollerComponent }
}
</script>

<style scoped>
.fade-enter-active,
.fade-leave-active {
  transition: opacity 0.2s ease;
}
.fade-enter-from,
.fade-leave-to {
  opacity: 0;
}
</style>
