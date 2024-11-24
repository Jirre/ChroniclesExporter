using ChroniclesExporter.IO.Database;

namespace ChroniclesExporter.Tables.Armor;

[DbEnum("armorCategories")]
public enum EArmorCategories
{
    unarmored,
    light,
    medium,
    heavy,
}