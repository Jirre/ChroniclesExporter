using ChroniclesExporter.IO.Database;
using Npgsql;

namespace ChroniclesExporter.Tables.Races;

public class RaceWriter : DbTableWriter<Race>
{
    protected override ETable TableId => ETable.Races;
    protected override string TableName => "races";
    protected override string[] Fields => ["id", "name", "content", "rarity", "ability", "sizes", "speed"];
    protected override async Task ImportRow(NpgsqlBinaryImporter pImporter, Race pData)
    {
        await pImporter.WriteAsync(pData.Id.ToByteArray(true));
        await pImporter.WriteAsync(pData.Name);
        await pImporter.WriteAsync(pData.Content);
        
        await pImporter.WriteAsync(pData.Rarity);
        await pImporter.WriteAsync(pData.Ability);
        await pImporter.WriteAsync(pData.Sizes);
        await pImporter.WriteAsync(pData.Speed);
    }
}
