using ChroniclesExporter.IO;
using ChroniclesExporter.IO.MySql;
using ChroniclesExporter.Table;

namespace ChroniclesExporter.Settings;

public interface ISettings
{
    string FilePath { get; }
    ETable[] Dependencies { get; }
    
    Type Reader { get; }
    Type Writer { get; }
}

public interface ISettings<in TData> : ISettings
    where TData : IRow
{
    string Url(TData pData);
    string LinkClasses(TData pData);
    string LinkIcon(TData pData);
}

public interface ISettings<in TData, TReader, TWriter> : ISettings<TData>
    where TData : IRow
    where TReader : IReader
    where TWriter : MySqlWriter<IRow>
{
    Type ISettings.Reader => typeof(TReader);
    Type ISettings.Writer => typeof(TWriter);
}
