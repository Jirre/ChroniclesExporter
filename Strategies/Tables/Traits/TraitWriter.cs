using ChroniclesExporter.IO.Database;
using Npgsql;

namespace ChroniclesExporter.Strategy.Traits;

public class TraitWriter : DbTableWriter<Trait>
{
    protected override ETable TableId => ETable.Traits;
    protected override string TableName => "traits";
    protected override string[] Fields => new[] {"id", "name", "priority", "content"};

    protected override async Task ImportRow(NpgsqlBinaryImporter pImporter, Trait pData)
    {
        await pImporter.WriteAsync(pData.Id.ToByteArray(true));
        await pImporter.WriteAsync(pData.Name);
        await pImporter.WriteAsync(pData.Priority);
        await pImporter.WriteAsync(pData.Content);
    }
}
