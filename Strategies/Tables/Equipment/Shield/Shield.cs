using ChroniclesExporter.Table;

namespace ChroniclesExporter.Tables.Shield;

public class Shield : IRow
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public string? Content { get; set; }
    
    public float? Cost { get; set; }
    public float? Weight { get; set; }
    
    public short AcBonus { get; set; }
    public short? MaxDex { get; set; }
    public short? MinStr { get; set; }
}
