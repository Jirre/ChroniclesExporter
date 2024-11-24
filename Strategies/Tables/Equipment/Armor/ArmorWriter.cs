using ChroniclesExporter.IO.Database;
using Npgsql;

namespace ChroniclesExporter.Tables.Armor;

public class ArmorWriter : DbTableWriter<Armor>
{
    protected override ETable TableId => ETable.Armor;
    protected override string TableName => "armors";

    protected override string[] Fields => new[]
        {"id", "name", "content", "category", "cost", "weight", "armor_class", "use_dex", "max_dex", "min_str", "speed_penalty"};
    protected override async Task ImportRow(NpgsqlBinaryImporter pImporter, Armor pData)
    {
        await pImporter.WriteAsync(pData.Id.ToByteArray(true));
        await pImporter.WriteAsync(pData.Name);
        await pImporter.WriteAsync(pData.Content);
        
        await pImporter.WriteAsync(pData.Category);
        await pImporter.WriteAsync(pData.Cost);
        await pImporter.WriteAsync(pData.Weight);
        await pImporter.WriteAsync(pData.Ac);
        await pImporter.WriteAsync(pData.AddDex);
        await pImporter.WriteAsync(pData.MaxDex);
        await pImporter.WriteAsync(pData.MinStr);
        await pImporter.WriteAsync(pData.SpeedPenalty);
    }
}
