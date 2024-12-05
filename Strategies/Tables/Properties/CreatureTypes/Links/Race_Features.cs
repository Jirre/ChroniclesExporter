using ChroniclesExporter.IO.Database;

namespace ChroniclesExporter.Strategy.CreatureTypes;

// ReSharper disable once InconsistentNaming
public class Race_Features : DbLinkWriter
{
    protected override ELink LinkId => ELink.CreatureTypesFeatures;

    protected override string TableName => "creaturetypes_features";
    protected override string[] Fields => ["creaturetype_id", "feature_id"];
}
