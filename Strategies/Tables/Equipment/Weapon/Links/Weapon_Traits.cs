using ChroniclesExporter.IO.Database;

namespace ChroniclesExporter.Strategy.Weapons;

// ReSharper disable once InconsistentNaming
public class Weapon_Traits : DbLinkWriter
{
    protected override ELink LinkId => ELink.WeaponTraits;

    protected override string TableName => "weapons_traits";
    protected override string[] Fields => new[] {"weapon_id", "trait_id"};
}
