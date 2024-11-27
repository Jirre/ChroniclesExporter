using ChroniclesExporter.IO.Database;

namespace ChroniclesExporter.Strategy.Armor;

// ReSharper disable once InconsistentNaming
public class Shield_Traits : DbLinkWriter
{
    protected override ELink LinkId => ELink.ShieldTraits;

    protected override string TableName => "shields_traits";
    protected override string[] Fields => new[] {"shield_id", "trait_id"};
}
