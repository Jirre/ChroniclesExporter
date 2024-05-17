using ChroniclesExporter.IO;

namespace ChroniclesExporter.Table;

public interface IParseSettings<TReader, TWriter>
    where TReader : IReader
    where TWriter : IWriter
{
    
}
