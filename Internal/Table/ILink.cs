namespace ChroniclesExporter.Table;

public interface ILink
{
    Guid Source { get; }
    Guid Target { get; }
}
