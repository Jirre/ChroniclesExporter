using ChroniclesExporter.IO.Database;

namespace ChroniclesExporter.Strategy.Races;

// ReSharper disable once InconsistentNaming
public class Race_Languages : DbLinkWriter
{
    protected override ELink LinkId => ELink.RaceLanguages;

    protected override string TableName => "races_languages";
    protected override string[] Fields => ["race_id", "language_id"];
}
