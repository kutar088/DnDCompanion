

const DICEBEAR_BASE = 'https://api.dicebear.com/7.x/adventurer/svg?seed='

export function avatarUrl(seedOrDataUri) {
  if (!seedOrDataUri) {
    return DICEBEAR_BASE + 'default'
  }
  if (typeof seedOrDataUri === 'string' && seedOrDataUri.startsWith('data:')) {
    return seedOrDataUri
  }
  return DICEBEAR_BASE + encodeURIComponent(seedOrDataUri)
}

export function isUploadedAvatar(seedOrDataUri) {
  return typeof seedOrDataUri === 'string' && seedOrDataUri.startsWith('data:')
}

export function fileToResizedDataUri(file, maxSize = 200, quality = 0.85) {
  return new Promise((resolve, reject) => {
    if (!file) return reject(new Error('Файл не обрано'))
    if (!file.type.startsWith('image/')) {
      return reject(new Error('Оберіть файл зображення'))
    }

    const reader = new FileReader()
    reader.onerror = () => reject(new Error('Не вдалося прочитати файл'))
    reader.onload = () => {
      const img = new Image()
      img.onerror = () => reject(new Error('Не вдалося обробити зображення'))
      img.onload = () => {
        const scale = Math.min(maxSize / img.width, maxSize / img.height, 1)
        const w = Math.round(img.width * scale)
        const h = Math.round(img.height * scale)

        const canvas = document.createElement('canvas')
        canvas.width = w
        canvas.height = h
        const ctx = canvas.getContext('2d')

ctx.fillStyle = '#ffffff'
        ctx.fillRect(0, 0, w, h)
        ctx.drawImage(img, 0, 0, w, h)

        try {
          resolve(canvas.toDataURL('image/jpeg', quality))
        } catch (e) {
          reject(e)
        }
      }
      img.src = reader.result
    }
    reader.readAsDataURL(file)
  })
}
