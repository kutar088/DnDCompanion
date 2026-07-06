<template>
  <div class="container-fluid my-4" style="max-width: 1400px;">
    
    <div class="d-flex justify-content-between align-items-center mb-3">
      <router-link to="/characters" class="btn btn-outline-light btn-sm">
        <i class="fa-solid fa-arrow-left"></i> До списку
      </router-link>
      <div class="d-flex gap-2">
        <button
          v-if="isEdit"
          class="btn btn-outline-danger btn-sm"
          @click="deleteCharacter"
        >
          <i class="fa-solid fa-trash"></i> Видалити
        </button>
        <button class="btn btn-dnd" @click="save" :disabled="saving">
          <i class="fa-solid fa-floppy-disk"></i>
          {{ saving ? 'Зберігаємо...' : (isEdit ? 'Зберегти' : 'Створити') }}
        </button>
      </div>
    </div>

    <div v-if="error" class="alert alert-danger">{{ error }}</div>

    <div class="character-sheet" data-aos="fade-up">
      
      <section class="sheet-block">
        <div class="row g-3 align-items-end">
          <div class="col-12 col-lg-5">
            <label class="sheet-label">Ім'я персонажа</label>
            <input
              v-model="form.name"
              type="text"
              class="form-control form-control-lg name-input"
              placeholder="Наприклад, Арагорн"
              maxlength="60"
              required
            />
          </div>
          <div class="col-6 col-md-3 col-lg-2">
            <label class="sheet-label">Раса</label>
            <select v-model="form.race" class="form-select">
              <option value="">—</option>
              <option v-for="r in races" :key="r.id" :value="r.name">{{ r.name }}</option>
            </select>
          </div>
          <div class="col-6 col-md-3 col-lg-2">
            <label class="sheet-label">Клас</label>
            <select v-model="form.class" class="form-select">
              <option value="">—</option>
              <option v-for="c in classes" :key="c.id" :value="c.name">{{ c.name }}</option>
            </select>
          </div>
          <div class="col-4 col-md-2 col-lg-1">
            <label class="sheet-label">Рівень</label>
            <input
              v-model.number="form.level"
              type="number"
              class="form-control text-center"
              min="1" max="20"
            />
          </div>
          <div class="col-8 col-md-4 col-lg-2 text-end">
            <div class="prof-badge">
              <div class="prof-badge-label">Бонус майстерності</div>
              <div class="prof-badge-value">+{{ profBonus }}</div>
            </div>
          </div>
        </div>

        <div class="row g-3 mt-2">
          <div class="col-12 col-md-6 col-lg-4">
            <label class="sheet-label">Передісторія</label>
            <select v-model="form.background" class="form-select">
              <option value="">—</option>
              <option v-for="b in BACKGROUNDS" :key="b" :value="b">{{ b }}</option>
            </select>
          </div>
          <div class="col-12 col-md-6 col-lg-4">
            <label class="sheet-label">Світогляд</label>
            <select v-model="form.alignment" class="form-select">
              <option value="">—</option>
              <option v-for="a in ALIGNMENTS" :key="a" :value="a">{{ a }}</option>
            </select>
          </div>
          <div class="col-12 col-lg-4">
            <div v-if="spellcastingAbility" class="spell-info">
              <i class="fa-solid fa-wand-sparkles"></i>
              <strong>Заклиначі:</strong> характеристика — {{ abilityLabels[spellcastingAbility] }},
              <span class="text-nowrap">СК рят. {{ spellSaveDC }}</span>,
              <span class="text-nowrap">атака {{ fmtMod(spellAttackBonus) }}</span>
            </div>
          </div>
        </div>
      </section>

<section class="sheet-block">
        <h3 class="section-title">
          <i class="fa-solid fa-dice-d20"></i> Характеристики та рятівні кидки
        </h3>
        <div class="row g-3">
          <div
            v-for="a in ABILITIES"
            :key="a.field"
            class="col-6 col-md-4 col-lg-2"
          >
            <div class="ability-box">
              <div class="ability-label">{{ a.label }}</div>
              <input
                v-model.number="form[a.field]"
                type="number"
                class="ability-score"
                min="1" max="30"
              />
              <div class="ability-mod">
                {{ fmtMod(mod(a.field)) }}
              </div>

              <div class="save-row" :class="{ prof: isSavingProficient(a.field) }">
                <span class="save-dot" :title="isSavingProficient(a.field) ? 'Володіє' : 'Не володіє'"></span>
                <span class="save-label">Рят.</span>
                <span class="save-value">{{ fmtMod(savingThrowMod(a.field)) }}</span>
              </div>
            </div>
          </div>
        </div>
      </section>

<section class="sheet-block">
        <h3 class="section-title">
          <i class="fa-solid fa-shield-halved"></i> Бойові показники
        </h3>
        <div class="row g-3">
          <div class="col-6 col-md-3 col-lg-2">
            <div class="combat-box">
              <div class="combat-label">
                <i class="fa-solid fa-shield" style="color: #6ea8fe;"></i>
                Клас захисту
              </div>
              <input v-model.number="form.armorClass" type="number" class="combat-value" min="0" max="30" />
            </div>
          </div>
          <div class="col-6 col-md-3 col-lg-2">
            <div class="combat-box">
              <div class="combat-label">
                <i class="fa-solid fa-bolt" style="color: #ffda6a;"></i>
                Ініціатива
              </div>
              <div class="combat-value read-only">{{ fmtMod(mod('dexterity')) }}</div>
              <small class="text-muted">= mod Спритності</small>
            </div>
          </div>
          <div class="col-6 col-md-3 col-lg-2">
            <div class="combat-box">
              <div class="combat-label">
                <i class="fa-solid fa-person-running" style="color: #75b798;"></i>
                Швидкість
              </div>
              <div class="combat-value read-only">{{ raceSpeed }}</div>
              <small class="text-muted">футів</small>
            </div>
          </div>
          <div class="col-6 col-md-3 col-lg-2">
            <div class="combat-box">
              <div class="combat-label">
                <i class="fa-solid fa-heart" style="color: var(--dnd-red);"></i>
                Макс HP
              </div>
              <input v-model.number="form.hitPoints" type="number" class="combat-value" min="1" />
            </div>
          </div>
          <div class="col-6 col-md-3 col-lg-2">
            <div class="combat-box">
              <div class="combat-label">
                <i class="fa-solid fa-dice" style="color: #adb5bd;"></i>
                Кістки життя
              </div>
              <div class="combat-value read-only">{{ form.level }}{{ hitDie }}</div>
              <small class="text-muted">= рівень × {{ hitDie }}</small>
            </div>
          </div>
          <div class="col-6 col-md-3 col-lg-2">
            <div class="combat-box">
              <div class="combat-label">
                <i class="fa-solid fa-eye" style="color: #d4af37;"></i>
                Passive Perception
              </div>
              <div class="combat-value read-only">{{ passivePerception }}</div>
              <small class="text-muted">= 10 + Сприйняття</small>
            </div>
          </div>
        </div>
      </section>

<section class="sheet-block">
        <h3 class="section-title">
          <i class="fa-solid fa-scroll"></i> Навички
          <small class="text-muted" style="font-size: 0.75rem; letter-spacing: 0;">
            (постав галочку — персонаж володіє навичкою)
          </small>
        </h3>
        <div class="row g-2">
          <div v-for="s in SKILLS" :key="s.id" class="col-12 col-md-6 col-lg-4">
            <label class="skill-row">
              <input
                type="checkbox"
                :value="s.id"
                v-model="proficientSkillsArray"
                class="form-check-input me-2"
              />
              <span class="skill-mod">{{ fmtMod(skillMod(s)) }}</span>
              <span class="skill-name">{{ s.name }}</span>
              <span class="skill-ability">({{ abilityShort(s.ability) }})</span>
            </label>
          </div>
        </div>
      </section>

<section class="sheet-block">
        <h3 class="section-title">
          <i class="fa-solid fa-mask"></i> Особистість
        </h3>
        <div class="row g-3">
          <div class="col-12 col-md-6">
            <label class="sheet-label">Риси характеру</label>
            <textarea v-model="form.personalityTraits" class="form-control" rows="3"
              placeholder="Що виділяє вашого персонажа серед інших?"></textarea>
          </div>
          <div class="col-12 col-md-6">
            <label class="sheet-label">Ідеали</label>
            <textarea v-model="form.ideals" class="form-control" rows="3"
              placeholder="У що вірить ваш персонаж?"></textarea>
          </div>
          <div class="col-12 col-md-6">
            <label class="sheet-label">Прив'язаності</label>
            <textarea v-model="form.bonds" class="form-control" rows="3"
              placeholder="Хто або що важливе для персонажа?"></textarea>
          </div>
          <div class="col-12 col-md-6">
            <label class="sheet-label">Слабкості</label>
            <textarea v-model="form.flaws" class="form-control" rows="3"
              placeholder="Які пороки чи страхи має ваш персонаж?"></textarea>
          </div>
        </div>
      </section>

<section class="sheet-block">
        <h3 class="section-title">
          <i class="fa-solid fa-book"></i> Біографія
        </h3>
        <textarea v-model="form.description" class="form-control" rows="6"
          placeholder="Розкажіть історію свого персонажа..."></textarea>
      </section>

<div class="d-flex justify-content-end gap-2 mt-4">
        <router-link to="/characters" class="btn btn-secondary">Скасувати</router-link>
        <button class="btn btn-dnd btn-lg" @click="save" :disabled="saving">
          <i class="fa-solid fa-floppy-disk"></i>
          {{ saving ? 'Зберігаємо...' : (isEdit ? 'Зберегти зміни' : 'Створити персонажа') }}
        </button>
      </div>
    </div>
  </div>
</template>

<script>
import { http, getHttpErrorMessage } from '@/interceptors/http.js'
import {
  abilityModifier, proficiencyBonus, fmtMod,
  classProficientSaves, classSpellcastingAbility, classHitDie,
  SKILLS, abilityShort
} from '@/utils/dnd.js'

const ABILITIES = [
  { label: 'Сила',         field: 'strength' },
  { label: 'Спритність',   field: 'dexterity' },
  { label: 'Витривалість', field: 'constitution' },
  { label: 'Інтелект',     field: 'intelligence' },
  { label: 'Мудрість',     field: 'wisdom' },
  { label: 'Харизма',      field: 'charisma' }
]

const ALIGNMENTS = [
  'Законно-добрий', 'Нейтрально-добрий', 'Хаотично-добрий',
  'Законно-нейтральний', 'Істинно нейтральний', 'Хаотично-нейтральний',
  'Законно-злий', 'Нейтрально-злий', 'Хаотично-злий'
]

const BACKGROUNDS = [
  'Акробат', 'Артист', 'Аристократ', 'Відлюдник', 'Гладіатор',
  'Злочинець', 'Матрос', 'Мудрець', 'Народний герой', 'Пілігрим',
  'Розвідник', 'Ремісник', 'Служитель', 'Солдат', 'Шарлатан'
]

function emptyForm() {
  return {
    name: '', race: '', class: '', level: 1, alignment: '', background: '',
    hitPoints: 10, armorClass: 10,
    strength: 10, dexterity: 10, constitution: 10,
    intelligence: 10, wisdom: 10, charisma: 10,
    personalityTraits: '', ideals: '', bonds: '', flaws: '',
    proficientSkills: '',
    description: ''
  }
}

export default {
  name: 'CharacterSheetView',
  data() {
    return {
      ABILITIES, ALIGNMENTS, BACKGROUNDS, SKILLS,
      abilityLabels: {
        strength: 'Сила', dexterity: 'Спритність', constitution: 'Витривалість',
        intelligence: 'Інтелект', wisdom: 'Мудрість', charisma: 'Харизма'
      },
      races: [], classes: [],
      form: emptyForm(),
      saving: false, error: ''
    }
  },
  computed: {
    isEdit() { return !!this.$route.params.id },
    profBonus() { return proficiencyBonus(this.form.level) },

    proficientSkillsArray: {
      get() {
        return (this.form.proficientSkills || '').split(',').filter(Boolean)
      },
      set(arr) {
        this.form.proficientSkills = arr.join(',')
      }
    },

    proficientSaves() { return classProficientSaves(this.form.class) },
    spellcastingAbility() { return classSpellcastingAbility(this.form.class) },
    hitDie() { return classHitDie(this.form.class) },

    raceSpeed() {
      const race = this.races.find(r => r.name === this.form.race)
      return race ? race.speed : 30
    },

    passivePerception() {
      const perceptionSkill = SKILLS.find(s => s.id === 'perception')
      return 10 + this.skillMod(perceptionSkill)
    },

    spellSaveDC() {
      if (!this.spellcastingAbility) return null
      return 8 + this.profBonus + this.mod(this.spellcastingAbility)
    },

    spellAttackBonus() {
      if (!this.spellcastingAbility) return null
      return this.profBonus + this.mod(this.spellcastingAbility)
    }
  },

  async mounted() {
    await this.loadReference()
    if (this.isEdit) {
      await this.loadCharacter(this.$route.params.id)
    }
  },

  methods: {
    fmtMod, abilityShort,

    mod(field) { return abilityModifier(this.form[field]) },

    isSavingProficient(field) {
      return this.proficientSaves.includes(field)
    },

    savingThrowMod(field) {
      let m = this.mod(field)
      if (this.isSavingProficient(field)) m += this.profBonus
      return m
    },

    skillMod(skill) {
      let m = abilityModifier(this.form[skill.ability])
      if (this.proficientSkillsArray.includes(skill.id)) {
        m += this.profBonus
      }
      return m
    },

    async loadReference() {
      try {
        const [r, c] = await Promise.all([http.get('/races'), http.get('/classes')])
        this.races = r.data
        this.classes = c.data
      } catch {  }
    },

    async loadCharacter(id) {
      try {
        const { data } = await http.get(`/characters/${id}`)
        this.form = { ...emptyForm(), ...data }
      } catch (e) {
        this.error = getHttpErrorMessage(e, 'Не вдалося завантажити персонажа.')
      }
    },

    async save() {
      if (!this.form.name.trim()) {
        this.error = "Введіть ім'я персонажа."
        return
      }
      this.error = ''
      this.saving = true
      try {
        if (this.isEdit) {
          await http.put(`/characters/${this.$route.params.id}`, this.form)
        } else {
          await http.post('/characters', this.form)
        }
        this.$router.push('/characters')
      } catch (e) {
        this.error = getHttpErrorMessage(e, 'Помилка збереження персонажа.')
      } finally {
        this.saving = false
      }
    },

    async deleteCharacter() {
      if (!confirm('Видалити персонажа? Це не можна скасувати.')) return
      try {
        await http.delete(`/characters/${this.$route.params.id}`)
        this.$router.push('/characters')
      } catch (e) {
        this.error = getHttpErrorMessage(e, 'Помилка видалення персонажа.')
      }
    }
  }
}
</script>

<style scoped>
.character-sheet {
  background:
    linear-gradient(135deg, rgba(200, 30, 30, 0.03), rgba(0, 0, 0, 0.15)),
    #2b3035;
  border: 2px solid rgba(200, 30, 30, 0.3);
  border-radius: 12px;
  padding: 1.5rem;
  box-shadow: 0 8px 25px rgba(0, 0, 0, 0.4);
}

.sheet-block {
  padding: 1rem 0;
  border-bottom: 1px solid rgba(255, 255, 255, 0.08);
}
.sheet-block:last-of-type { border-bottom: none; }

.section-title {
  font-family: 'Georgia', serif;
  color: var(--dnd-red);
  margin-bottom: 1rem;
  padding-bottom: 0.5rem;
  border-bottom: 2px solid var(--dnd-red);
  font-size: 1.3rem;
  letter-spacing: 0.02em;
}

.sheet-label {
  font-size: 0.72rem;
  letter-spacing: 0.1em;
  text-transform: uppercase;
  color: #adb5bd;
  margin-bottom: 0.35rem;
  display: block;
  font-weight: 600;
}

.name-input {
  font-family: 'Georgia', serif;
  font-size: 1.6rem;
  font-weight: bold;
  background: transparent;
  border-bottom: 2px solid var(--dnd-red);
  border-top: none; border-left: none; border-right: none;
  border-radius: 0;
  color: white;
  padding-left: 0;
}
.name-input:focus {
  background: transparent; color: white; box-shadow: none;
  border-bottom-color: var(--dnd-red);
}

.prof-badge {
  display: inline-block;
  background: var(--dnd-red);
  color: white;
  border-radius: 10px;
  padding: 6px 12px;
  text-align: center;
  min-width: 130px;
}
.prof-badge-label {
  font-size: 0.65rem;
  text-transform: uppercase;
  letter-spacing: 0.05em;
  opacity: 0.85;
}
.prof-badge-value {
  font-size: 1.4rem;
  font-weight: bold;
  font-family: 'Georgia', serif;
  line-height: 1;
}

.ability-box {
  background: #1a1d20;
  border: 2px solid var(--dnd-red);
  border-radius: 10px;
  padding: 0.75rem 0.5rem;
  text-align: center;
  transition: transform 0.15s ease;
}
.ability-box:hover { transform: translateY(-2px); }
.ability-label {
  font-family: 'Georgia', serif;
  font-size: 0.85rem;
  text-transform: uppercase;
  color: #adb5bd;
  letter-spacing: 0.05em;
  margin-bottom: 0.35rem;
}
.ability-score {
  background: transparent; border: none; color: white;
  font-size: 2.3rem; font-weight: bold; text-align: center;
  width: 100%; padding: 0; font-family: 'Georgia', serif;
}
.ability-score:focus { outline: none; color: var(--dnd-red); }
.ability-mod {
  background: var(--dnd-red); color: white;
  border-radius: 20px; padding: 2px 10px;
  font-family: 'Courier New', monospace; font-weight: bold;
  display: inline-block; margin-top: 0.35rem; font-size: 0.9rem;
}

.save-row {
  margin-top: 0.5rem;
  padding-top: 0.5rem;
  border-top: 1px dashed rgba(255, 255, 255, 0.1);
  font-size: 0.8rem;
  display: flex;
  align-items: center;
  justify-content: center;
  gap: 6px;
  color: #adb5bd;
}
.save-row.prof { color: #f5f5f5; }
.save-dot {
  width: 10px; height: 10px; border-radius: 50%;
  background: transparent;
  border: 1px solid #adb5bd;
  display: inline-block;
}
.save-row.prof .save-dot {
  background: var(--dnd-red);
  border-color: var(--dnd-red);
}
.save-value {
  font-weight: bold;
  font-family: 'Courier New', monospace;
}

.combat-box {
  background: #1a1d20;
  border: 1px solid rgba(255, 255, 255, 0.15);
  border-radius: 8px;
  padding: 0.75rem;
  text-align: center;
}
.combat-label {
  font-size: 0.7rem;
  text-transform: uppercase;
  color: #adb5bd;
  letter-spacing: 0.05em;
  margin-bottom: 0.35rem;
}
.combat-value {
  background: transparent; border: none; color: white;
  font-size: 1.8rem; font-weight: bold; text-align: center;
  width: 100%; padding: 0; font-family: 'Georgia', serif; line-height: 1.1;
}
.combat-value:focus { outline: none; color: var(--dnd-red); }
.combat-value.read-only { cursor: default; }

.skill-row {
  display: flex; align-items: center; gap: 4px;
  padding: 4px 8px;
  border-radius: 5px;
  cursor: pointer;
  font-size: 0.9rem;
  transition: background 0.1s ease;
}
.skill-row:hover { background: rgba(255, 255, 255, 0.04); }
.skill-mod {
  min-width: 32px;
  text-align: center;
  font-family: 'Courier New', monospace;
  font-weight: bold;
  color: var(--dnd-red);
}
.skill-name { flex: 1; }
.skill-ability {
  color: #adb5bd;
  font-size: 0.75rem;
  text-transform: uppercase;
}

.spell-info {
  background: rgba(200, 30, 30, 0.1);
  border-left: 3px solid var(--dnd-red);
  padding: 8px 12px;
  border-radius: 4px;
  font-size: 0.85rem;
}
</style>
