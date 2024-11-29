using ChroniclesExporter.IO.Database;
using Npgsql;

namespace ChroniclesExporter.Strategy.CreatureTypes;

public class CreatureTypeWriter : DbTableWriter<CreatureType>
{
    protected override ETable TableId => ETable.CreatureTypes;
    protected override string TableName => "creaturetypes";
    protected override string[] Fields => new[] {"id", "name", "content", "parent_id"};

    protected override async Task ImportRow(NpgsqlBinaryImporter pImporter, CreatureType pData)
    {
        await pImporter.WriteAsync(pData.Id.ToByteArray(true));
        await pImporter.WriteAsync(pData.Name);
        await pImporter.WriteAsync(pData.Content);
        await pImporter.WriteAsync(pData.Parent?.ToByteArray(true));
    }
}
