using ChroniclesExporter.Settings;
using ChroniclesExporter.Table;

namespace ChroniclesExporter.Tables.Equipment.Weapon;

[Settings(ETable.Weapon)]
public class WeaponSettings : ISettings<WeaponReader, WeaponWriter>
{
    public string FilePath => "Equipment/Weapons";
    public string Url(IRow pData)
    {
        return "/weapons?id={0}";
    }

    public string LinkIcon(IRow pData)
    {
        if (pData is not Weapon weapon)
            return "sword";
        
        return weapon.Categories.Contains(EWeaponCategories.Ranged) ? "bow" : "sword";
    }

    public string LinkIconClasses(IRow pData)
    {
        if (pData is not Weapon weapon)
            return string.Empty;
        
        return weapon.Proficiency switch
        {
            EWeaponProficiencies.Simple => "grey",
            EWeaponProficiencies.Martial => "green",
            EWeaponProficiencies.Exotic => "purple",
            EWeaponProficiencies.Firearm => "orange",
            _ => throw new ArgumentOutOfRangeException($"Unknown Proficiency: {weapon.Proficiency}")
        };
    }

    public ETable[] Dependencies =>
    [
        ETable.Traits
    ];
}
