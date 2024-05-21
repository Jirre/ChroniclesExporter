using ChroniclesExporter.Table;

namespace ChroniclesExporter.Strategy.Links;

public class Link(Guid pSource, Guid pTarget) : ILink
{
    public Guid Source { get; } = pSource;
    public Guid Target { get; } = pTarget;
}
