using ChroniclesExporter.IO.Database;
using Npgsql;

namespace ChroniclesExporter.Tables.Languages;

public class LanguageWriter : DbTableWriter<Language>
{
    protected override ETable TableId => ETable.Languages;
    protected override string TableName => "languages";
    protected override string[] Fields => ["id", "name", "content", "rarity", "script_id"];

    protected override async Task ImportRow(NpgsqlBinaryImporter pImporter, Language pData)
    {
        await pImporter.WriteAsync(pData.Id.ToByteArray(true));
        await pImporter.WriteAsync(pData.Name);
        await pImporter.WriteAsync(pData.Content);
        await pImporter.WriteAsync(pData.Rarity);
        await pImporter.WriteAsync(pData.Script);
    }
}
