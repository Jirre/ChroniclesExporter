using ChroniclesExporter.IO.Database;
using Npgsql;

namespace ChroniclesExporter.Tables.Shield;

public class ShieldWriter : DbTableWriter<Shield>
{
    protected override ETable TableId => ETable.Shield;
    protected override string TableName => "shields";

    protected override string[] Fields => ["id", "name", "content", "cost", "weight", "armor_bonus", "max_dex", "min_str"];
    protected override async Task ImportRow(NpgsqlBinaryImporter pImporter, Shield pData)
    {
        await pImporter.WriteAsync(pData.Id.ToByteArray(true));
        await pImporter.WriteAsync(pData.Name);
        await pImporter.WriteAsync(pData.Content);
        
        await pImporter.WriteAsync(pData.Cost);
        await pImporter.WriteAsync(pData.Weight);
        await pImporter.WriteAsync(pData.AcBonus);
        await pImporter.WriteAsync(pData.MaxDex);
        await pImporter.WriteAsync(pData.MinStr);
    }
}
