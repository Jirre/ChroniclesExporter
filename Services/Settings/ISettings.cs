using ChroniclesExporter.IO;
using ChroniclesExporter.IO.Database;
using ChroniclesExporter.Table;

namespace ChroniclesExporter.Settings;

public interface ISettings
{
    string FilePath { get; }
    ETable[] Dependencies { get; }

    Type Reader { get; }
    Type Writer { get; }
    
    string Url(IRow pData);
    string LinkClasses(IRow pData) => "";
    string LinkIcon(IRow pData);
    string LinkIconClasses(IRow pData) => "";
}

public interface ISettings<TReader, TWriter> : ISettings
    where TReader : IReader
    where TWriter : DbWriter<IRow>
{
    Type ISettings.Reader => typeof(TReader);
    Type ISettings.Writer => typeof(TWriter);
}
