

export function abilityModifier(score) {
  const n = Number(score) || 0
  return Math.floor((n - 10) / 2)
}

export function proficiencyBonus(level) {
  const lvl = Math.max(1, Math.min(20, Number(level) || 1))
  return 2 + Math.floor((lvl - 1) / 4)
}

export function fmtMod(mod) {
  if (mod > 0) return `+${mod}`
  if (mod < 0) return `${mod}`
  return '0'
}

const CLASS_SAVES = {
  'Воїн':        ['strength', 'constitution'],
  'Чарівник':    ['intelligence', 'wisdom'],
  'Шахрай':      ['dexterity', 'intelligence'],
  'Клірик':      ['wisdom', 'charisma'],
  'Варвар':      ['strength', 'constitution'],
  'Бард':        ['dexterity', 'charisma'],
  'Друїд':       ['intelligence', 'wisdom'],
  'Паладин':     ['wisdom', 'charisma'],
  'Слідопит':    ['strength', 'dexterity'],
  'Чародій':     ['constitution', 'charisma'],
  'Чорнокнижник':['wisdom', 'charisma'],
  'Монах':       ['strength', 'dexterity']
}

const CLASS_CASTING = {
  'Бард':        'charisma',
  'Клірик':      'wisdom',
  'Друїд':       'wisdom',
  'Паладин':     'charisma',
  'Слідопит':    'wisdom',
  'Чародій':     'charisma',
  'Чорнокнижник':'charisma',
  'Чарівник':    'intelligence'
}

const CLASS_HIT_DIE = {
  'Воїн': 'd10', 'Чарівник': 'd6', 'Шахрай': 'd8', 'Клірик': 'd8',
  'Варвар': 'd12', 'Бард': 'd8', 'Друїд': 'd8', 'Паладин': 'd10',
  'Слідопит': 'd10', 'Чародій': 'd6', 'Чорнокнижник': 'd8', 'Монах': 'd8'
}

export function classProficientSaves(className) {
  return CLASS_SAVES[className] || []
}

export function classSpellcastingAbility(className) {
  return CLASS_CASTING[className] || null
}

export function classHitDie(className) {
  return CLASS_HIT_DIE[className] || 'd8'
}

export const SKILLS = [
  { id: 'acrobatics',     name: 'Акробатика',       ability: 'dexterity' },
  { id: 'animal-handling',name: 'Тваринництво',     ability: 'wisdom' },
  { id: 'arcana',         name: 'Аркана',           ability: 'intelligence' },
  { id: 'athletics',      name: 'Атлетика',         ability: 'strength' },
  { id: 'deception',      name: 'Обман',            ability: 'charisma' },
  { id: 'history',        name: 'Історія',          ability: 'intelligence' },
  { id: 'insight',        name: 'Проникливість',    ability: 'wisdom' },
  { id: 'intimidation',   name: 'Залякування',      ability: 'charisma' },
  { id: 'investigation',  name: 'Розслідування',    ability: 'intelligence' },
  { id: 'medicine',       name: 'Медицина',         ability: 'wisdom' },
  { id: 'nature',         name: 'Природа',          ability: 'intelligence' },
  { id: 'perception',     name: 'Сприйняття',       ability: 'wisdom' },
  { id: 'performance',    name: 'Виступ',           ability: 'charisma' },
  { id: 'persuasion',     name: 'Переконання',      ability: 'charisma' },
  { id: 'religion',       name: 'Релігія',          ability: 'intelligence' },
  { id: 'sleight-of-hand',name: 'Спритність рук',   ability: 'dexterity' },
  { id: 'stealth',        name: 'Скритність',       ability: 'dexterity' },
  { id: 'survival',       name: 'Виживання',        ability: 'wisdom' }
]

export function abilityShort(abilityField) {
  return {
    strength:     'Сил',
    dexterity:    'Спр',
    constitution: 'Вит',
    intelligence: 'Інт',
    wisdom:       'Мдр',
    charisma:     'Хар'
  }[abilityField] || abilityField
}
