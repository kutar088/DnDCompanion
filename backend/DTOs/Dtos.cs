namespace backend.DTOs;

public record RegisterDto(string Name, string Email, string Password);

public record LoginDto(string Email, string Password);

public record UpdateProfileDto(string Name, string AvatarSeed);

public record ChangePasswordDto(string OldPassword, string NewPassword);

public record CharacterDto(
    string Name,
    string Race,
    string Class,
    int Level,
    int HitPoints,
    int Strength,
    int Dexterity,
    int Constitution,
    int Intelligence,
    int Wisdom,
    int Charisma,
    string Description,
    int ArmorClass = 10,
    string Background = "",
    string Alignment = "",
    string PersonalityTraits = "",
    string Ideals = "",
    string Bonds = "",
    string Flaws = "",
    string ProficientSkills = ""
);
