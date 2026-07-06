<template>
  <footer class="text-center text-muted py-4 mt-auto">
    <small>
      &copy; {{ year }} DnDCompanion — MVP навчальної практики
      <span class="mx-2">·</span>
      <i class="fa-solid fa-dice-d20" style="color: var(--dnd-red);"></i>
    </small>
  </footer>
</template>

<script>
export default {
  name: 'FooterComponent',
  computed: {
    year() { return new Date().getFullYear() }
  }
}
</script>
