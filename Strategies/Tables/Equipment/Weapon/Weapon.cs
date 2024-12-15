using ChroniclesExporter.Table;

namespace ChroniclesExporter.Tables.Equipment.Weapon;

public class Weapon : IRow
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public string? Content { get; set; }

    public EWeaponProficiencies Proficiency { get; set; }
    public EWeaponGrips Grip { get; set; }
    public required EWeaponCategories[] Categories { get; set; }

    public string? Damage { get; set; }
    public EDamageTypes? DamageType { get; set; }
    public short? Range { get; set; }
    
    public float? Cost { get; set; }
    public float? Weight { get; set; }
}
