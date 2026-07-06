namespace backend.Models;

public class DndRace
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Size { get; set; } = "Medium";
    public int Speed { get; set; } = 30;
    public string AbilityBonuses { get; set; } = string.Empty;
    public string Traits { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}
