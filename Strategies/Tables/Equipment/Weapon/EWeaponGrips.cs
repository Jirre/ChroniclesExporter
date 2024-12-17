using ChroniclesExporter.IO.Database;

namespace ChroniclesExporter.Tables.Equipment.Weapon;

[DbEnum("weaponGrips")]
public enum EWeaponGrips
{
    Light,
    OneHanded,
    TwoHanded,
    Versatile,
    Double
}
