using ChroniclesExporter.Table;

namespace ChroniclesExporter.Tables.Races;

public class Race : IRow
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string? Content { get; set; }
    
    public ERarities Rarity { get; set; }
    public EAbilities? Ability { get; set; }
    public required ESizes[] Sizes { get; set; }
    public short Speed { get; set; }
}
