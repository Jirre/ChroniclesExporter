using ChroniclesExporter.IO.Database;

namespace ChroniclesExporter.Strategy.Traits;

[DbEnum("traitCategories")]
public enum ETraitCategories
{
    condition,
    language,
    movementType,
    CreatureType,
    senses,
    skill,
    spell,
    spellComponent,
    spellSchool,
    weapon,
}
