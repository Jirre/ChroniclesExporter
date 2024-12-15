using ChroniclesExporter.IO.Database;

namespace ChroniclesExporter.Tables.Equipment.Weapon;

[DbEnum("weaponCategories")]
public enum EWeaponCategories
{
    Melee,
    Ranged,
    Thrown
}