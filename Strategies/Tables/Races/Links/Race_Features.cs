using ChroniclesExporter.IO.Database;

namespace ChroniclesExporter.Strategy.Races;

// ReSharper disable once InconsistentNaming
public class Race_Features : DbLinkWriter
{
    protected override ELink LinkId => ELink.RaceFeatures;

    protected override string TableName => "races_features";
    protected override string[] Fields => ["race_id", "feature_id"];
}
