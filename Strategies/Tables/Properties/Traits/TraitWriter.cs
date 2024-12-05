using ChroniclesExporter.IO.Database;
using Npgsql;

namespace ChroniclesExporter.Strategy.Traits;

public class TraitWriter : DbTableWriter<Trait>
{
    protected override ETable TableId => ETable.Traits;
    protected override string TableName => "traits";
    protected override string[] Fields => ["id", "name", "priority", "content", "categories", "icon", "class"];

    protected override async Task ImportRow(NpgsqlBinaryImporter pImporter, Trait pData)
    {
        await pImporter.WriteAsync(pData.Id.ToByteArray(true));
        await pImporter.WriteAsync(pData.Name);
        await pImporter.WriteAsync(pData.Priority);
        await pImporter.WriteAsync(pData.Content);
        await pImporter.WriteAsync(pData.Categories);
        await pImporter.WriteAsync(pData.Icon);
        await pImporter.WriteAsync(pData.Class);
    }
}
