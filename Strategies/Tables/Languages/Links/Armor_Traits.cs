using ChroniclesExporter.IO.Database;

namespace ChroniclesExporter.Strategy.Language;

// ReSharper disable once InconsistentNaming
public class Language_Traits : DbLinkWriter
{
    protected override ELink LinkId => ELink.LanguageTraits;

    protected override string TableName => "languages_traits";
    protected override string[] Fields => new[] {"language_id", "trait_id"};
}
