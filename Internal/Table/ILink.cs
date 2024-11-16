namespace ChroniclesExporter.Table;

public interface ILink
{
    Guid Source { get; }
    object Target { get; }
}