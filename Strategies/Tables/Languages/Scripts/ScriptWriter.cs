using ChroniclesExporter.IO.Database;
using Npgsql;

namespace ChroniclesExporter.Tables.Languages;

public class ScriptWriter : DbTableWriter<Script>
{
    protected override ETable TableId => ETable.LanguageScripts;
    protected override string TableName => "language" + "scripts";
    protected override string[] Fields => new[] {"id", "name", "content"};

    protected override async Task ImportRow(NpgsqlBinaryImporter pImporter, Script pData)
    {
        await pImporter.WriteAsync(pData.Id.ToByteArray(true));
        await pImporter.WriteAsync(pData.Name);
        await pImporter.WriteAsync(pData.Content);
    }
}
