<template>
  <div class="container my-4">
    <h2 class="dnd-title mb-4">
      <i class="fa-solid fa-user-pen"></i> Профіль
    </h2>

    <div class="row g-4">
      
      <div class="col-12 col-lg-4">
        <div class="card" data-aos="fade-up">
          <div class="card-body text-center">
            <img
              :src="avatarUrl(profileForm.avatarSeed || auth.user?.avatarSeed)"
              alt="avatar"
              class="rounded-circle mb-3 border border-3 avatar-preview"
              width="140"
              height="140"
              style="border-color: var(--dnd-red) !important; background: #fff;"
            />
            <transition name="fade">
              <div v-if="avatarSavedFlash" class="avatar-saved-flash">
                <i class="fa-solid fa-check-circle text-success"></i> Збережено
              </div>
            </transition>
            <h4 class="dnd-title">{{ auth.user?.name }}</h4>
            <p class="text-muted mb-0">
              <i class="fa-solid fa-envelope"></i> {{ auth.user?.email }}
            </p>
            <p class="text-muted small">
              <i class="fa-solid fa-user-shield"></i> Роль: {{ auth.user?.role }}
            </p>

            <hr />

<h6 class="text-start mb-2">
              <i class="fa-solid fa-cloud-arrow-up"></i> Завантажити своє фото
            </h6>
            <input
              ref="fileInput"
              type="file"
              accept="image/*"
              class="d-none"
              @change="onFileSelected"
            />
            <div class="d-flex gap-2 mb-3">
              <button
                type="button"
                class="btn btn-outline-light btn-sm flex-grow-1"
                :disabled="uploading"
                @click="$refs.fileInput.click()"
              >
                <i class="fa-solid fa-image"></i>
                {{ uploading ? 'Обробка...' : 'Обрати файл' }}
              </button>
              <button
                v-if="isUploaded(profileForm.avatarSeed)"
                type="button"
                class="btn btn-outline-danger btn-sm"
                title="Прибрати завантажене фото"
                @click="clearUpload"
              >
                <i class="fa-solid fa-xmark"></i>
              </button>
            </div>
            <p class="text-muted small mb-0" v-if="uploadError">
              <span class="text-danger">{{ uploadError }}</span>
            </p>
            <p class="text-muted small mb-0" v-else>
              JPG / PNG. Зображення автоматично зменшиться до 200×200 px.
            </p>

            <hr />

<h6 class="text-start mb-3">
              <i class="fa-solid fa-mask"></i> Або обрати готовий аватар
            </h6>
            <div class="d-flex flex-wrap gap-2 justify-content-center">
              <button
                v-for="seed in avatarSeeds"
                :key="seed"
                type="button"
                class="btn p-1 avatar-picker"
                :class="{ selected: profileForm.avatarSeed === seed }"
                @click="pickPreset(seed)"
              >
                <img
                  :src="dicebearUrl(seed)"
                  alt="avatar preview"
                  width="48"
                  height="48"
                  class="rounded-circle"
                  style="background: #fff;"
                />
              </button>
            </div>
          </div>
        </div>
      </div>

<div class="col-12 col-lg-8">
        <div class="card mb-4" data-aos="fade-up">
          <div class="card-body">
            <h5 class="mb-3">
              <i class="fa-solid fa-id-card"></i> Основні дані
            </h5>

            <div v-if="profileMsg" class="alert" :class="'alert-' + profileMsgType">
              {{ profileMsg }}
            </div>

            <form @submit.prevent="saveProfile">
              <div class="mb-3">
                <label class="form-label">Ім'я</label>
                <input
                  v-model="profileForm.name"
                  type="text"
                  class="form-control"
                  required
                />
              </div>

              <button type="submit" class="btn btn-dnd" :disabled="profileSaving">
                {{ profileSaving ? 'Зберігаємо...' : 'Зберегти' }}
              </button>
            </form>
          </div>
        </div>

        <div class="card" data-aos="fade-up" data-aos-delay="100">
          <div class="card-body">
            <h5 class="mb-3">
              <i class="fa-solid fa-key"></i> Зміна пароля
            </h5>

            <div v-if="passMsg" class="alert" :class="'alert-' + passMsgType">
              {{ passMsg }}
            </div>

            <form @submit.prevent="savePassword">
              <div class="mb-3">
                <label class="form-label">Поточний пароль</label>
                <input
                  v-model="passForm.oldPassword"
                  type="password"
                  class="form-control"
                  required
                />
              </div>
              <div class="mb-3">
                <label class="form-label">Новий пароль</label>
                <input
                  v-model="passForm.newPassword"
                  type="password"
                  class="form-control"
                  minlength="6"
                  required
                />
              </div>
              <div class="mb-3">
                <label class="form-label">Підтвердження</label>
                <input
                  v-model="passForm.confirmPassword"
                  type="password"
                  class="form-control"
                  minlength="6"
                  required
                />
              </div>

              <button
                type="submit"
                class="btn btn-outline-warning"
                :disabled="passSaving"
              >
                {{ passSaving ? 'Змінюємо...' : 'Змінити пароль' }}
              </button>
            </form>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import { useAuthStore } from '@/stores/auth'
import { getHttpErrorMessage } from '@/interceptors/http.js'
import { avatarUrl, isUploadedAvatar, fileToResizedDataUri } from '@/utils/avatar.js'

const AVATAR_SEEDS = [
  'Aragorn', 'Legolas', 'Gandalf', 'Frodo', 'Gimli',
  'Thorin', 'Bilbo', 'Arwen', 'Boromir', 'Galadriel',
  'Elrond', 'Sauron', 'Merlin', 'Morgana', 'Loki'
]

export default {
  name: 'ProfileView',
  data() {
    return {
      auth: useAuthStore(),
      avatarSeeds: AVATAR_SEEDS,
      profileForm: { name: '', avatarSeed: '' },
      profileSaving: false,
      profileMsg: '',
      profileMsgType: 'success',
      uploading: false,
      uploadError: '',
      avatarSavedFlash: false,
      passForm: { oldPassword: '', newPassword: '', confirmPassword: '' },
      passSaving: false,
      passMsg: '',
      passMsgType: 'success'
    }
  },
  mounted() {
    this.profileForm.name = this.auth.user?.name || ''
    this.profileForm.avatarSeed = this.auth.user?.avatarSeed || ''
  },
  methods: {
    avatarUrl,
    isUploaded: isUploadedAvatar,

    dicebearUrl(seed) {
      return `https://api.dicebear.com/7.x/adventurer/svg?seed=${encodeURIComponent(seed)}`
    },

    async onFileSelected(event) {
      const file = event.target.files?.[0]
      if (!file) return

      this.uploadError = ''
      this.uploading = true
      try {
        const dataUri = await fileToResizedDataUri(file, 200, 0.85)
        this.profileForm.avatarSeed = dataUri
        await this.persistAvatar(dataUri)
      } catch (e) {
        this.uploadError = e?.message || 'Не вдалося обробити зображення.'
      } finally {
        this.uploading = false
        
        event.target.value = ''
      }
    },

    async pickPreset(seed) {
      this.profileForm.avatarSeed = seed
      await this.persistAvatar(seed)
    },

    async clearUpload() {
      this.profileForm.avatarSeed = ''
      await this.persistAvatar('')
    },

    async persistAvatar(value) {
      try {
        await this.auth.updateProfile({
          name: this.profileForm.name || this.auth.user?.name || '',
          avatarSeed: value
        })
        this.flashAvatarSaved()
      } catch (e) {
        this.uploadError = getHttpErrorMessage(e, 'Не вдалося зберегти аватар.')
      }
    },

    flashAvatarSaved() {
      this.avatarSavedFlash = true
      clearTimeout(this._flashTimer)
      this._flashTimer = setTimeout(() => { this.avatarSavedFlash = false }, 1600)
    },

    async saveProfile() {
      this.profileMsg = ''
      this.profileSaving = true
      try {
        await this.auth.updateProfile({
          name: this.profileForm.name,
          avatarSeed: this.profileForm.avatarSeed
        })
        this.profileMsgType = 'success'
        this.profileMsg = 'Профіль оновлено.'
      } catch (e) {
        this.profileMsgType = 'danger'
        this.profileMsg = getHttpErrorMessage(e, 'Помилка збереження.')
      } finally {
        this.profileSaving = false
      }
    },

    async savePassword() {
      this.passMsg = ''
      if (this.passForm.newPassword !== this.passForm.confirmPassword) {
        this.passMsgType = 'danger'
        this.passMsg = 'Паролі не збігаються.'
        return
      }
      this.passSaving = true
      try {
        await this.auth.changePassword({
          oldPassword: this.passForm.oldPassword,
          newPassword: this.passForm.newPassword
        })
        this.passMsgType = 'success'
        this.passMsg = 'Пароль успішно змінено.'
        this.passForm = { oldPassword: '', newPassword: '', confirmPassword: '' }
      } catch (e) {
        this.passMsgType = 'danger'
        this.passMsg = getHttpErrorMessage(e, 'Помилка зміни пароля.')
      } finally {
        this.passSaving = false
      }
    }
  }
}
</script>

<style scoped>
.avatar-preview {
  object-fit: cover;
}

.avatar-saved-flash {
  font-size: 0.85rem;
  color: #75b798;
  margin-top: -0.5rem;
  margin-bottom: 0.5rem;
}

.fade-enter-active, .fade-leave-active {
  transition: opacity 0.25s ease;
}
.fade-enter-from, .fade-leave-to {
  opacity: 0;
}

.avatar-picker {
  border: 2px solid transparent;
  border-radius: 50%;
  transition: transform 0.1s ease;
  padding: 2px !important;
}

.avatar-picker:hover {
  transform: scale(1.1);
}

.avatar-picker.selected {
  border-color: var(--dnd-red);
  transform: scale(1.05);
}
</style>
