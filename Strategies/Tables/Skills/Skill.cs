using ChroniclesExporter.Table;

namespace ChroniclesExporter.Strategy.Traits;

public class Skill : IRow
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public EAbilities Ability { get; set; }
    public string? Content { get; set; }
}
