using ChroniclesExporter.IO.Database;

namespace ChroniclesExporter.Strategy.Traits;

[DbEnum("traitCategories")]
public enum ETraitCategories
{
    condition,
    language,
    movementType,
    creatureType,
    senses,
    skill,
    spell,
    spellComponent,
    spellSchool,
    
    // Equipment
    armor,
    weapon,
}
