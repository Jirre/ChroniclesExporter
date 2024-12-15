using ChroniclesExporter.IO.Database;

namespace ChroniclesExporter;

[DbEnum("damageTypes")]
public enum EDamageTypes
{
    Acid,
    Bludgeoning,
    Cold,
    Fire,
    Force,
    Holy,
    Lightning,
    Necrotic,
    Piercing,
    Poison,
    Psychic,
    Slashing,
    Sonic,
    Unholy
}
