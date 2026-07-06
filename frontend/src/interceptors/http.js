import axios from 'axios'

const API_BASE_URL = import.meta.env.VITE_API_BASE_URL || '/api'

export const http = axios.create({
  baseURL: API_BASE_URL
})

let authTokenGetter = null
let unauthorizedHandler = null
let handlingUnauthorized = false

export function setAuthTokenGetter(getter) {
  authTokenGetter = typeof getter === 'function' ? getter : null
}

export function setUnauthorizedHandler(handler) {
  unauthorizedHandler = typeof handler === 'function' ? handler : null
}

http.interceptors.request.use((config) => {
  const token = authTokenGetter ? authTokenGetter() : ''
  if (token && !config.headers?.Authorization) {
    config.headers = {
      ...config.headers,
      Authorization: `Bearer ${token}`
    }
  }
  return config
})

http.interceptors.response.use(
  (response) => response,
  async (error) => {
    if (error.response?.status === 401 && unauthorizedHandler && !handlingUnauthorized) {
      handlingUnauthorized = true
      try {
        unauthorizedHandler()
      } finally {
        handlingUnauthorized = false
      }
    }
    return Promise.reject(error)
  }
)

export function getHttpErrorMessage(error, fallback = 'Сталася помилка запиту.') {
  if (!error) return fallback
  if (error.response) {
    const data = error.response.data
    if (typeof data === 'string' && data.trim()) return data
    if (data?.error) return data.error
    if (data?.message) return data.message
    switch (error.response.status) {
      case 400: return 'Некоректний запит.'
      case 401: return 'Потрібна авторизація.'
      case 403: return 'Немає доступу.'
      case 404: return 'Не знайдено.'
      case 409: return 'Конфлікт даних.'
      case 500: return 'Внутрішня помилка сервера.'
      default: return fallback
    }
  }
  return error.message || fallback
}
