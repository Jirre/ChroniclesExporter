using ChroniclesExporter.IO.Database;

namespace ChroniclesExporter.Strategy.Armor;

// ReSharper disable once InconsistentNaming
public class Armor_Traits : DbLinkWriter
{
    protected override ELink LinkId => ELink.ArmorTraits;

    protected override string TableName => "armors_traits";
    protected override string[] Fields => new[] {"armor_id", "trait_id"};
}
