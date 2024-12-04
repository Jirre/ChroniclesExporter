using ChroniclesExporter.IO.Database;

namespace ChroniclesExporter.Strategy.Traits;

[DbEnum("traitCategories")]
public enum ETraitCategories
{
    Condition,
    Language,
    MovementType,
    CreatureType,
    Senses,
    Skill,
    Spell,
    SpellComponent,
    SpellSchool,
    
    // Equipment
    Armor,
    Weapon,
}
