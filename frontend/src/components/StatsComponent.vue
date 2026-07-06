<template>
  <section class="my-5" data-aos="fade-up">
    <div class="row g-4 text-center">
      <div class="col-6 col-md-3">
        <div class="stat-block">
          <div class="stat-number">{{ counts.races }}</div>
          <div class="stat-label">Рас</div>
        </div>
      </div>
      <div class="col-6 col-md-3">
        <div class="stat-block">
          <div class="stat-number">{{ counts.classes }}</div>
          <div class="stat-label">Класів</div>
        </div>
      </div>
      <div class="col-6 col-md-3">
        <div class="stat-block">
          <div class="stat-number">{{ counts.spells }}</div>
          <div class="stat-label">Заклинань</div>
        </div>
      </div>
      <div class="col-6 col-md-3">
        <div class="stat-block">
          <div class="stat-number">∞</div>
          <div class="stat-label">Персонажів</div>
        </div>
      </div>
    </div>
  </section>
</template>

<script>
import { http } from '@/interceptors/http.js'

export default {
  name: 'StatsComponent',
  data() {
    return { counts: { races: '·', classes: '·', spells: '·' } }
  },
  async mounted() {
    try {
      const [r, c, s] = await Promise.all([
        http.get('/races'),
        http.get('/classes'),
        http.get('/spells')
      ])
      this.counts = { races: r.data.length, classes: c.data.length, spells: s.data.length }
    } catch {  }
  }
}
</script>

<style scoped>
.stat-block {
  padding: 1.5rem 0;
}
.stat-number {
  font-size: 3.5rem;
  font-weight: bold;
  color: var(--dnd-red);
  font-family: 'Georgia', serif;
  line-height: 1;
}
.stat-label {
  font-size: 1rem;
  text-transform: uppercase;
  letter-spacing: 0.1em;
  color: #adb5bd;
  margin-top: 0.5rem;
}
</style>
