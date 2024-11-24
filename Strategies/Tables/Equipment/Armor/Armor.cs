using ChroniclesExporter.Table;

namespace ChroniclesExporter.Tables.Armor;

public class Armor : IRow
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public string? Content { get; set; }
    
    public EArmorCategories Category { get; set; }
    public float? Cost { get; set; }
    public float? Weight { get; set; }
    
    public short Ac { get; set; }
    public bool AddDex { get; set; }
    public short? MaxDex { get; set; }
    
    public short? MinStr { get; set; }
    public short? SpeedPenalty { get; set; }
}
