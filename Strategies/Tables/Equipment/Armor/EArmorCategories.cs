using ChroniclesExporter.IO.Database;

namespace ChroniclesExporter.Tables.Armor;

[DbEnum("armorCategories")]
public enum EArmorCategories
{
    Unarmored,
    Light,
    Medium,
    Heavy,
}