using ChroniclesExporter.IO;
using ChroniclesExporter.Utility;

namespace ChroniclesExporter.Tables.Equipment.Weapon;

public class WeaponReader : MdReader<Weapon>
{
    protected override bool TryGetProperties(string pLine, Weapon pData)
    {
        return MarkdownUtility.TryParseEnum<EWeaponProficiencies>(pLine, "Proficiency:", e => pData.Proficiency = e) ||
               MarkdownUtility.TryParseEnum<EWeaponGrips>(pLine, "Grip:", e => pData.Grip = e) ||
               MarkdownUtility.TryParseEnumArray<EWeaponCategories>(pLine, "Category:", e => pData.Categories = e) ||
               MarkdownUtility.TryParseEnum<EDamageTypes>(pLine, "Type:", e => pData.DamageType = e) ||
               MarkdownUtility.TryParseString(pLine, "Damage:", e => pData.Damage = e) ||
               MarkdownUtility.TryParseNumber<short>(pLine, "Range:", e => pData.Range = e) ||
               MarkdownUtility.TryParseNumber<float>(pLine, "Cost:", e => pData.Cost = e,
                   System.Globalization.NumberStyles.Float) ||
               MarkdownUtility.TryParseNumber<float>(pLine, "Weight:", e => pData.Weight = e,
                   System.Globalization.NumberStyles.Float) ||
               MarkdownUtility.TryRegisterLinks(pLine, "Traits:", ELink.WeaponTraits, pData.Id);
    }
}
