using ChroniclesExporter.IO.Database;

namespace ChroniclesExporter.Strategy.Races;

// ReSharper disable once InconsistentNaming
public class Race_Traits : DbLinkWriter
{
    protected override ELink LinkId => ELink.RaceTraits;

    protected override string TableName => "races_traits";
    protected override string[] Fields => ["race_id", "trait_id"];
}
