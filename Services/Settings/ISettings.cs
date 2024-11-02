using ChroniclesExporter.IO;
using ChroniclesExporter.IO.MySql;
using ChroniclesExporter.Table;

namespace ChroniclesExporter.Settings;

public interface ISettings
{
    string FilePath { get; }
    string Url { get; }
    string LinkClasses { get; }
    string LinkIcon { get; }
    
    ETable[] Dependencies { get; }
    
    Type Reader { get; }
    Type Writer { get; }
}

public interface ISettings<TReader, TWriter> : ISettings
    where TReader : IReader
    where TWriter : MySqlWriter<IRow>
{
    Type ISettings.Reader => typeof(TReader);
    Type ISettings.Writer => typeof(TWriter);
}
