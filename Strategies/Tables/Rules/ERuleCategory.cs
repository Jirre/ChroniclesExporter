using ChroniclesExporter.IO.Database;

namespace ChroniclesExporter.Tables.Rules;

[DbEnum("ruleCategories")]
public enum ERuleCategory
{
    Basics,
    Combat,
    Exploration,
    Spells,
}
