using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<Character> Characters => Set<Character>();
    public DbSet<DndRace> Races => Set<DndRace>();
    public DbSet<DndClass> Classes => Set<DndClass>();
    public DbSet<DndSpell> Spells => Set<DndSpell>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();

        modelBuilder.Entity<Character>()
            .HasOne(c => c.User)
            .WithMany(u => u.Characters)
            .HasForeignKey(c => c.UserId)
            .OnDelete(DeleteBehavior.Cascade);

modelBuilder.Entity<DndRace>().HasData(
            new DndRace
            {
                Id = 1, Name = "Людина", Size = "Середній", Speed = 30,
                AbilityBonuses = "+1 до всіх характеристик",
                Traits = "Універсальність, Додаткова мова",
                Description = "Універсальні й амбітні, люди — найпоширеніша раса у більшості світів D&D."
            },
            new DndRace
            {
                Id = 2, Name = "Ельф", Size = "Середній", Speed = 30,
                AbilityBonuses = "+2 до Спритності",
                Traits = "Темний зір, Гострі чуття, Фейська кров, Транс",
                Description = "Витончені й довгожителі, ельфи — магічний народ неземної грації."
            },
            new DndRace
            {
                Id = 3, Name = "Дварф", Size = "Середній", Speed = 25,
                AbilityBonuses = "+2 до Витривалості",
                Traits = "Темний зір, Дварфська стійкість, Кам'янознавство",
                Description = "Сміливі й витривалі, дварфи — вправні воїни, шахтарі та каменярі."
            },
            new DndRace
            {
                Id = 4, Name = "Гобіт", Size = "Малий", Speed = 25,
                AbilityBonuses = "+2 до Спритності",
                Traits = "Щасливчик, Хоробрий, Гобітська спритність",
                Description = "Малі й практичні, гобіти віддають перевагу домашньому затишку, а не пригодам."
            },
            new DndRace
            {
                Id = 5, Name = "Драконороджений", Size = "Середній", Speed = 30,
                AbilityBonuses = "+2 до Сили, +1 до Харизми",
                Traits = "Драконячий родовід, Подих-зброя, Опір урону",
                Description = "Народжені від драконів, вони гордо крокують світом, який зустрічає їх із трепетом."
            },
            new DndRace
            {
                Id = 6, Name = "Тифлінг", Size = "Середній", Speed = 30,
                AbilityBonuses = "+2 до Харизми, +1 до Інтелекту",
                Traits = "Темний зір, Пекельний опір, Інфернальна спадщина",
                Description = "Нащадки бісів, тифлінги несуть інфернальну кров, що дає їм силу."
            }
        );

modelBuilder.Entity<DndClass>().HasData(
            new DndClass { Id = 1, Name = "Воїн", HitDie = "d10",
                PrimaryAbility = "Сила або Спритність",
                SavingThrows = "Сила, Витривалість",
                Description = "Майстер бойового мистецтва, вправний із різноманітною зброєю та обладунками." },
            new DndClass { Id = 2, Name = "Чарівник", HitDie = "d6",
                PrimaryAbility = "Інтелект",
                SavingThrows = "Інтелект, Мудрість",
                Description = "Учений заклинач, здатний маніпулювати структурою реальності." },
            new DndClass { Id = 3, Name = "Шахрай", HitDie = "d8",
                PrimaryAbility = "Спритність",
                SavingThrows = "Спритність, Інтелект",
                Description = "Пройдисвіт, що використовує скритність і хитрість, щоб долати перешкоди й ворогів." },
            new DndClass { Id = 4, Name = "Клірик", HitDie = "d8",
                PrimaryAbility = "Мудрість",
                SavingThrows = "Мудрість, Харизма",
                Description = "Служитель, що володіє божественною магією на службі вищої сили." },
            new DndClass { Id = 5, Name = "Варвар", HitDie = "d12",
                PrimaryAbility = "Сила",
                SavingThrows = "Сила, Витривалість",
                Description = "Лютий воїн, здатний увійти в бойову лють." },
            new DndClass { Id = 6, Name = "Бард", HitDie = "d8",
                PrimaryAbility = "Харизма",
                SavingThrows = "Спритність, Харизма",
                Description = "Натхненний чарівник, чия сила відлунює музикою творіння." },
            new DndClass { Id = 7, Name = "Друїд", HitDie = "d8",
                PrimaryAbility = "Мудрість",
                SavingThrows = "Інтелект, Мудрість",
                Description = "Жрець Старої віри, володар сил природи." },
            new DndClass { Id = 8, Name = "Паладин", HitDie = "d10",
                PrimaryAbility = "Сила, Харизма",
                SavingThrows = "Мудрість, Харизма",
                Description = "Святий воїн, зв'язаний священною присягою." },
            new DndClass { Id = 9, Name = "Слідопит", HitDie = "d10",
                PrimaryAbility = "Спритність, Мудрість",
                SavingThrows = "Сила, Спритність",
                Description = "Воїн, що бореться з загрозами на межі цивілізації." },
            new DndClass { Id = 10, Name = "Чародій", HitDie = "d6",
                PrimaryAbility = "Харизма",
                SavingThrows = "Витривалість, Харизма",
                Description = "Заклинач, що спирається на вроджену магію дару або крові." },
            new DndClass { Id = 11, Name = "Чорнокнижник", HitDie = "d8",
                PrimaryAbility = "Харизма",
                SavingThrows = "Мудрість, Харизма",
                Description = "Носій магії, отриманої від угоди з потойбічною сутністю." },
            new DndClass { Id = 12, Name = "Монах", HitDie = "d8",
                PrimaryAbility = "Спритність, Мудрість",
                SavingThrows = "Сила, Спритність",
                Description = "Майстер бойових мистецтв, що черпає силу з тіла." }
        );

modelBuilder.Entity<DndSpell>().HasData(
            new DndSpell { Id = 1, Name = "Вогняна стріла", Level = 0, School = "Викликання", CastingTime = "1 дія", Range = "120 футів", Components = "В, С", Duration = "Миттєва", Classes = "Чародій, Чарівник", Description = "Ви жбурляєте вогняну іскру в істоту або об'єкт у межах дальності. Зробіть далекобійну атаку заклинанням. При влучанні ціль отримує 1d10 урону вогнем." },
            new DndSpell { Id = 2, Name = "Містичний вибух", Level = 0, School = "Викликання", CastingTime = "1 дія", Range = "120 футів", Components = "В, С", Duration = "Миттєва", Classes = "Чорнокнижник", Description = "Промінь потріскуючої енергії летить у ціль. Зробіть далекобійну атаку заклинанням. При влучанні — 1d10 урону силовою енергією." },
            new DndSpell { Id = 3, Name = "Рука мага", Level = 0, School = "Прикликання", CastingTime = "1 дія", Range = "30 футів", Components = "В, С", Duration = "1 хвилина", Classes = "Бард, Чародій, Чорнокнижник, Чарівник", Description = "Примарна летюча рука з'являється в точці на ваш вибір у межах дальності. Ви можете використовувати дію для керування нею." },
            new DndSpell { Id = 4, Name = "Фокус", Level = 0, School = "Перетворення", CastingTime = "1 дія", Range = "10 футів", Components = "В, С", Duration = "До 1 години", Classes = "Бард, Чародій, Чорнокнижник, Чарівник", Description = "Незначний магічний трюк, який новачки-заклиначі використовують для тренування." },
            new DndSpell { Id = 5, Name = "Магічна стріла", Level = 1, School = "Викликання", CastingTime = "1 дія", Range = "120 футів", Components = "В, С", Duration = "Миттєва", Classes = "Чародій, Чарівник", Description = "Ви створюєте три сяючі дротики магічної сили. Кожен вражає обрану істоту на 1d4+1 силового урону." },
            new DndSpell { Id = 6, Name = "Зцілення ран", Level = 1, School = "Викликання", CastingTime = "1 дія", Range = "Дотик", Components = "В, С", Duration = "Миттєва", Classes = "Бард, Клірик, Друїд, Паладин, Слідопит", Description = "Істота, до якої ви торкаєтеся, відновлює хіти дорівнює 1d8 + ваш модифікатор характеристики заклинань." },
            new DndSpell { Id = 7, Name = "Щит", Level = 1, School = "Захисна", CastingTime = "1 реакція", Range = "На себе", Components = "В, С", Duration = "1 раунд", Classes = "Чародій, Чарівник", Description = "Невидимий бар'єр магічної сили захищає вас. До початку вашого наступного ходу ви маєте +5 до КЗ." },
            new DndSpell { Id = 8, Name = "Обладунок мага", Level = 1, School = "Захисна", CastingTime = "1 дія", Range = "Дотик", Components = "В, С, М", Duration = "8 годин", Classes = "Чародій, Чарівник", Description = "Ви торкаєтеся охочої істоти. Її базовий КЗ стає 13 + її модифікатор Спритності. Розсіюється при вдяганні обладунків." },
            new DndSpell { Id = 9, Name = "Виявлення магії", Level = 1, School = "Ворожіння", CastingTime = "1 дія", Range = "На себе", Components = "В, С", Duration = "До 10 хвилин", Classes = "Бард, Клірик, Друїд, Паладин, Слідопит, Чародій, Чарівник", Description = "Протягом тривалості ви відчуваєте присутність магії в радіусі 30 футів." },
            new DndSpell { Id = 10, Name = "Сон", Level = 1, School = "Зачарування", CastingTime = "1 дія", Range = "90 футів", Components = "В, С, М", Duration = "1 хвилина", Classes = "Бард, Чародій, Чарівник", Description = "Заклинання відправляє істот у магічний сон. Киньте 5d8 — це загальна кількість хітів істот, на які може вплинути заклинання." },
            new DndSpell { Id = 11, Name = "Зачарування особи", Level = 1, School = "Зачарування", CastingTime = "1 дія", Range = "30 футів", Components = "В, С", Duration = "1 година", Classes = "Бард, Друїд, Чародій, Чорнокнижник, Чарівник", Description = "Ви намагаєтеся зачарувати гуманоїда, якого бачите в межах дальності. Він має пройти рятівний кидок Мудрості." },
            new DndSpell { Id = 12, Name = "Туманний крок", Level = 2, School = "Прикликання", CastingTime = "1 бонусна дія", Range = "На себе", Components = "В", Duration = "Миттєва", Classes = "Чародій, Чорнокнижник, Чарівник", Description = "На мить оточені сріблястим туманом, ви телепортуєтеся до 30 футів у вільний простір, який бачите." },
            new DndSpell { Id = 13, Name = "Павутина", Level = 2, School = "Прикликання", CastingTime = "1 дія", Range = "60 футів", Components = "В, С, М", Duration = "До 1 години", Classes = "Чародій, Чарівник", Description = "Ви заклинаєте масу товстої липкої павутини в обраній точці в межах дальності." },
            new DndSpell { Id = 14, Name = "Невидимість", Level = 2, School = "Ілюзія", CastingTime = "1 дія", Range = "Дотик", Components = "В, С, М", Duration = "До 1 години", Classes = "Бард, Чародій, Чорнокнижник, Чарівник", Description = "Істота, до якої ви торкаєтеся, стає невидимою до закінчення заклинання." },
            new DndSpell { Id = 15, Name = "Утримання особи", Level = 2, School = "Зачарування", CastingTime = "1 дія", Range = "60 футів", Components = "В, С, М", Duration = "До 1 хвилини", Classes = "Бард, Клірик, Друїд, Чародій, Чорнокнижник, Чарівник", Description = "Оберіть гуманоїда, якого бачите в межах дальності. Ціль має пройти рятівний кидок Мудрості або буде паралізована." },
            new DndSpell { Id = 16, Name = "Вогняний шар", Level = 3, School = "Викликання", CastingTime = "1 дія", Range = "150 футів", Components = "В, С, М", Duration = "Миттєва", Classes = "Чародій, Чарівник", Description = "Яскравий спалах летить у обрану точку і вибухає. Кожна істота в сфері радіусом 20 футів отримує 8d6 урону вогнем." },
            new DndSpell { Id = 17, Name = "Контрзаклинання", Level = 3, School = "Захисна", CastingTime = "1 реакція", Range = "60 футів", Components = "С", Duration = "Миттєва", Classes = "Чародій, Чорнокнижник, Чарівник", Description = "Ви намагаєтеся перервати істоту в процесі створення заклинання." },
            new DndSpell { Id = 18, Name = "Блискавка", Level = 3, School = "Викликання", CastingTime = "1 дія", Range = "На себе (лінія 100 футів)", Components = "В, С, М", Duration = "Миттєва", Classes = "Чародій, Чарівник", Description = "Блискавка формує лінію 100 футів завдовжки та 5 футів завширшки. 8d6 урону електрикою." },
            new DndSpell { Id = 19, Name = "Прискорення", Level = 3, School = "Перетворення", CastingTime = "1 дія", Range = "30 футів", Components = "В, С, М", Duration = "До 1 хвилини", Classes = "Чародій, Чарівник", Description = "Швидкість цілі подвоюється, +2 до КЗ, перевага на кидки Спритності, додаткова дія." },
            new DndSpell { Id = 20, Name = "Політ", Level = 3, School = "Перетворення", CastingTime = "1 дія", Range = "Дотик", Components = "В, С, М", Duration = "До 10 хвилин", Classes = "Чародій, Чорнокнижник, Чарівник", Description = "Ви торкаєтеся охочої істоти. Ціль отримує швидкість польоту 60 футів на тривалість заклинання." },
            new DndSpell { Id = 21, Name = "Перевтілення", Level = 4, School = "Перетворення", CastingTime = "1 дія", Range = "60 футів", Components = "В, С, М", Duration = "До 1 години", Classes = "Бард, Друїд, Чародій, Чарівник", Description = "Перетворює істоту на нову форму. Триває до кінця заклинання або поки ціль не отримає 0 хітів." },
            new DndSpell { Id = 22, Name = "Стіна вогню", Level = 4, School = "Викликання", CastingTime = "1 дія", Range = "120 футів", Components = "В, С, М", Duration = "До 1 хвилини", Classes = "Друїд, Чародій, Чарівник", Description = "Ви створюєте вогняну стіну на твердій поверхні. 5d8 урону вогнем при провалі рятівного кидка." },
            new DndSpell { Id = 23, Name = "Конус холоду", Level = 5, School = "Викликання", CastingTime = "1 дія", Range = "На себе (конус 60 футів)", Components = "В, С, М", Duration = "Миттєва", Classes = "Чародій, Чарівник", Description = "З ваших рук вивергається порив холодного повітря. Кожна істота в конусі 60 футів отримує 8d8 урону холодом." },
            new DndSpell { Id = 24, Name = "Метеоритний дощ", Level = 9, School = "Викликання", CastingTime = "1 дія", Range = "1 миля", Components = "В, С", Duration = "Миттєва", Classes = "Чародій, Чарівник", Description = "Палаючі кулі вогню падають на землю. Кожна істота в сфері радіусом 40 футів отримує 20d6 урону вогнем та 20d6 удару." },
            new DndSpell { Id = 25, Name = "Бажання", Level = 9, School = "Прикликання", CastingTime = "1 дія", Range = "На себе", Components = "В", Duration = "Миттєва", Classes = "Чародій, Чарівник", Description = "Наймогутніше заклинання, яке може створити смертний. Просто вимовивши вголос, ви можете змінити самі основи реальності відповідно до ваших бажань." }
        );
    }
}
