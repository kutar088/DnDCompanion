<template>
  <div class="container my-5">
    <div class="row justify-content-center">
      <div class="col-12 col-md-6 col-lg-5">
        <LoginComponent />
      </div>
    </div>
  </div>
</template>

<script>
import LoginComponent from '@/components/LoginComponent.vue'
export default { name: 'LoginView', components: { LoginComponent } }
</script>
