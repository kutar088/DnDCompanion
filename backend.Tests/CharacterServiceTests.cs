using backend.DTOs;
using backend.Models;
using backend.Services;
using Xunit;

namespace backend.Tests;

public class CharacterServiceTests
{

[Theory]
    [InlineData(10, 0)]
    [InlineData(12, 1)]
    [InlineData(8, -1)]
    [InlineData(20, 5)]
    [InlineData(1, -5)]
    [InlineData(30, 10)]
    public void GetAbilityModifier_ReturnsCorrectValue(int score, int expected)
    {
        var result = CharacterService.GetAbilityModifier(score);
        Assert.Equal(expected, result);
    }

[Fact]
    public void IsOwnedByUser_ReturnsTrue_WhenUserIdMatches()
    {
        var character = new Character { UserId = 42 };
        Assert.True(CharacterService.IsOwnedByUser(character, 42));
    }

    [Fact]
    public void IsOwnedByUser_ReturnsFalse_WhenUserIdDiffers()
    {
        var character = new Character { UserId = 42 };
        Assert.False(CharacterService.IsOwnedByUser(character, 7));
    }

    [Fact]
    public void IsOwnedByUser_ThrowsOnNullCharacter()
    {
        Assert.Throws<ArgumentNullException>(() =>
            CharacterService.IsOwnedByUser(null!, 1));
    }

[Fact]
    public void LevelUp_IncrementsLevel_WhenBelow20()
    {
        var character = new Character { Level = 5 };
        var newLevel = CharacterService.LevelUp(character);

        Assert.Equal(6, newLevel);
        Assert.Equal(6, character.Level);
    }

    [Fact]
    public void LevelUp_CapsAt20()
    {
        var character = new Character { Level = 20 };
        var newLevel = CharacterService.LevelUp(character);

        Assert.Equal(20, newLevel);
        Assert.Equal(20, character.Level);
    }

[Fact]
    public void IsValidNewCharacter_ReturnsTrue_ForValidDto()
    {
        var dto = new CharacterDto(
            "Aragorn", "Human", "Ranger",
            5, 40,
            16, 14, 14, 12, 13, 11,
            "Heir of Isildur.");
        Assert.True(CharacterService.IsValidNewCharacter(dto));
    }

    [Fact]
    public void IsValidNewCharacter_ReturnsFalse_ForEmptyName()
    {
        var dto = new CharacterDto(
            "", "Human", "Fighter",
            1, 10,
            10, 10, 10, 10, 10, 10,
            "");
        Assert.False(CharacterService.IsValidNewCharacter(dto));
    }

    [Fact]
    public void IsValidNewCharacter_ReturnsFalse_ForLevelAbove20()
    {
        var dto = new CharacterDto(
            "Boromir", "Human", "Fighter",
            25, 100,
            15, 12, 14, 10, 11, 13,
            "");
        Assert.False(CharacterService.IsValidNewCharacter(dto));
    }

    [Fact]
    public void IsValidNewCharacter_ReturnsFalse_ForAbilityScoreOutOfRange()
    {
        var dto = new CharacterDto(
            "Legolas", "Elf", "Ranger",
            5, 40,
            50, 20, 14, 12, 15, 13,
            "");
        Assert.False(CharacterService.IsValidNewCharacter(dto));
    }
}
