using backend.DTOs;
using backend.Models;

namespace backend.Services;

public static class CharacterService
{

public static int GetAbilityModifier(int abilityScore)
    {
        return (int)Math.Floor((abilityScore - 10) / 2.0);
    }

public static bool IsOwnedByUser(Character character, int userId)
    {
        ArgumentNullException.ThrowIfNull(character);
        return character.UserId == userId;
    }

public static int LevelUp(Character character)
    {
        ArgumentNullException.ThrowIfNull(character);
        if (character.Level < 20)
        {
            character.Level++;
        }
        return character.Level;
    }

public static bool IsValidNewCharacter(CharacterDto dto)
    {
        if (dto is null) return false;
        if (string.IsNullOrWhiteSpace(dto.Name)) return false;
        if (dto.Name.Length > 60) return false;
        if (dto.Level is < 1 or > 20) return false;
        if (dto.HitPoints < 1) return false;

        return AllAbilityScoresValid(dto);
    }

public static bool AllAbilityScoresValid(CharacterDto dto)
    {
        int[] scores =
        {
            dto.Strength, dto.Dexterity, dto.Constitution,
            dto.Intelligence, dto.Wisdom, dto.Charisma
        };
        return scores.All(s => s is >= 1 and <= 30);
    }
}
