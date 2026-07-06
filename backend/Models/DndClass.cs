namespace backend.Models;

public class DndClass
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string HitDie { get; set; } = "d8";
    public string PrimaryAbility { get; set; } = string.Empty;
    public string SavingThrows { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}
