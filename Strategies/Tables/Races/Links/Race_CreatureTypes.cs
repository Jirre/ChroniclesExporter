using ChroniclesExporter.IO.Database;

namespace ChroniclesExporter.Strategy.Races;

// ReSharper disable once InconsistentNaming
public class Race_CreatureTypes : DbLinkWriter
{
    protected override ELink LinkId => ELink.RaceCreatureTypes;

    protected override string TableName => "races_creaturetypes";
    protected override string[] Fields => ["race_id", "creaturetype_id"];
}
