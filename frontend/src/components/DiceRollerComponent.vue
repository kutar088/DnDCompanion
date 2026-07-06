<template>
  <div>
    
    <button
      class="btn dice-fab shadow-lg"
      @click="open"
      title="Кинути кубики"
    >
      <i class="fa-solid fa-dice-d20"></i>
    </button>

<div class="modal fade" tabindex="-1" ref="modalEl">
      <div class="modal-dialog modal-lg">
        <div class="modal-content">
          <div class="modal-header">
            <h5 class="modal-title dnd-title">
              <i class="fa-solid fa-dice-d20" style="color: var(--dnd-red);"></i>
              Кубики
            </h5>
            <button
              type="button"
              class="btn-close btn-close-white"
              data-bs-dismiss="modal"
            ></button>
          </div>

          <div class="modal-body">
            
            <div class="d-flex flex-wrap gap-2 justify-content-center mb-3">
              <button
                v-for="d in diceSides"
                :key="d"
                class="btn dice-btn"
                :class="selectedDie === d ? 'btn-dnd' : 'btn-outline-light'"
                @click="selectedDie = d"
              >
                d{{ d }}
              </button>
            </div>

<div class="row g-3 align-items-end mb-3">
              <div class="col-md-3">
                <label class="form-label">Кількість</label>
                <input
                  v-model.number="count"
                  type="number"
                  class="form-control"
                  min="1"
                  max="20"
                />
              </div>
              <div class="col-md-3">
                <label class="form-label">Модифікатор</label>
                <input
                  v-model.number="modifier"
                  type="number"
                  class="form-control"
                />
              </div>
              <div class="col-md-6">
                <button
                  class="btn btn-dnd w-100 btn-lg"
                  @click="roll"
                  :disabled="rolling"
                >
                  <i
                    class="fa-solid fa-dice-d20"
                    :class="{ 'fa-spin': rolling }"
                  ></i>
                  Кинути {{ count }}d{{ selectedDie
                  }}{{ modifierLabel }}
                </button>
              </div>
            </div>

<div v-if="lastResult" class="text-center py-4" data-aos="zoom-in">
              <div class="dice-result-total">
                {{ lastResult.total }}
              </div>
              <div class="text-muted small">
                <span
                  v-for="(r, i) in lastResult.rolls"
                  :key="i"
                  class="badge bg-secondary mx-1"
                >
                  {{ r }}
                </span>
                <span v-if="lastResult.modifier">
                  {{ lastResult.modifier > 0 ? '+' : ''
                  }}{{ lastResult.modifier }}
                </span>
                <span class="ms-2 text-muted">
                  = {{ lastResult.formula }}
                </span>
              </div>
            </div>

<div v-if="history.length" class="mt-4">
              <h6 class="d-flex justify-content-between align-items-center">
                Історія
                <button class="btn btn-sm btn-outline-danger" @click="clearHistory">
                  <i class="fa-solid fa-trash"></i>
                </button>
              </h6>
              <ul class="list-group">
                <li
                  v-for="(h, i) in history.slice(0, 10)"
                  :key="i"
                  class="list-group-item d-flex justify-content-between align-items-center"
                >
                  <span>
                    <strong>{{ h.formula }}</strong>
                    <small class="text-muted ms-2">
                      [{{ h.rolls.join(', ') }}]
                    </small>
                  </span>
                  <span
                    class="badge fs-6"
                    style="background-color: var(--dnd-red);"
                  >
                    {{ h.total }}
                  </span>
                </li>
              </ul>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import { Modal } from 'bootstrap'

const STORAGE_KEY = 'dnd_dice_history'

export default {
  name: 'DiceRollerComponent',
  data() {
    return {
      modal: null,
      diceSides: [4, 6, 8, 10, 12, 20, 100],
      selectedDie: 20,
      count: 1,
      modifier: 0,
      rolling: false,
      lastResult: null,
      history: this.loadHistory()
    }
  },
  computed: {
    modifierLabel() {
      if (!this.modifier) return ''
      return this.modifier > 0 ? `+${this.modifier}` : `${this.modifier}`
    }
  },
  mounted() {
    this.modal = new Modal(this.$refs.modalEl)
  },
  methods: {
    open() {
      this.modal.show()
    },

    roll() {
      this.rolling = true
      setTimeout(() => {
        const rolls = []
        for (let i = 0; i < this.count; i++) {
          rolls.push(Math.floor(Math.random() * this.selectedDie) + 1)
        }
        const total = rolls.reduce((a, b) => a + b, 0) + this.modifier

        const result = {
          rolls,
          modifier: this.modifier,
          total,
          formula: `${this.count}d${this.selectedDie}${this.modifierLabel}`,
          timestamp: Date.now()
        }

        this.lastResult = result
        this.history.unshift(result)
        this.history = this.history.slice(0, 25)
        this.saveHistory()
        this.rolling = false
      }, 350)
    },

    clearHistory() {
      this.history = []
      this.saveHistory()
    },

    loadHistory() {
      try {
        const raw = localStorage.getItem(STORAGE_KEY)
        return raw ? JSON.parse(raw) : []
      } catch {
        return []
      }
    },

    saveHistory() {
      try {
        localStorage.setItem(STORAGE_KEY, JSON.stringify(this.history))
      } catch {
        
      }
    }
  }
}
</script>

<style scoped>
.dice-fab {
  position: fixed;
  bottom: 24px;
  right: 24px;
  width: 60px;
  height: 60px;
  border-radius: 50%;
  background-color: var(--dnd-red);
  color: white;
  font-size: 1.5rem;
  border: none;
  z-index: 1030;
  display: flex;
  align-items: center;
  justify-content: center;
  transition: transform 0.15s ease;
}

.dice-fab:hover {
  transform: scale(1.1) rotate(15deg);
  background-color: var(--dnd-red-dark);
  color: white;
}

.dice-btn {
  min-width: 60px;
  font-family: 'Courier New', monospace;
  font-weight: bold;
}

.dice-result-total {
  font-size: 4rem;
  font-weight: bold;
  color: var(--dnd-red);
  font-family: 'Georgia', serif;
  line-height: 1;
}
</style>
