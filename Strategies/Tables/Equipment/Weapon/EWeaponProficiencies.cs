using ChroniclesExporter.IO.Database;

namespace ChroniclesExporter.Tables.Equipment.Weapon;

[DbEnum("weaponProficiencies")]
public enum EWeaponProficiencies
{
    Simple,
    Martial,
    Exotic,
    Firearm
}
