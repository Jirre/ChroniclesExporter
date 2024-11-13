using ChroniclesExporter.IO.Database;

namespace ChroniclesExporter.Strategy.Links;

// ReSharper disable once InconsistentNaming
public class Condition_Traits : DbLinkWriter<Link>
{
    protected override ELink LinkId => ELink.ConditionTraits;

    protected override string TableName => "conditions_traits";
    protected override string[] Fields => new[] {"condition_id", "trait_id"};
}
