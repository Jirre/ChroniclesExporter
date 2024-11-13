using ChroniclesExporter.IO.Database;

namespace ChroniclesExporter.Strategy.Traits;

[DbEnum("traitCategories")]
public enum ETraitCategories
{
    Condition,
    Language,
    MovementType,
    Race_CreatureType,
    Race_Size,
    Senses,
    Skill,
    Spell_Component,
    Spell_Property,
    Spell_School,
    Weapon_Property
}
