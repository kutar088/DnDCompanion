<template>
  <div class="container my-5">
    <div class="row justify-content-center">
      <div class="col-12 col-md-6 col-lg-5">
        <RegisterComponent />
      </div>
    </div>
  </div>
</template>

<script>
import RegisterComponent from '@/components/RegisterComponent.vue'
export default { name: 'RegisterView', components: { RegisterComponent } }
</script>
