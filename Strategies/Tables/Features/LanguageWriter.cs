using ChroniclesExporter.IO.Database;
using Npgsql;

namespace ChroniclesExporter.Tables.Features;

public class FeatureWriter : DbTableWriter<Feature>
{
    protected override ETable TableId => ETable.Features;
    protected override string TableName => "features";
    protected override string[] Fields => ["id", "name", "content", "priority"];

    protected override async Task ImportRow(NpgsqlBinaryImporter pImporter, Feature pData)
    {
        await pImporter.WriteAsync(pData.Id.ToByteArray(true));
        await pImporter.WriteAsync(pData.Name);
        await pImporter.WriteAsync(pData.Content);
        await pImporter.WriteAsync(pData.Index ?? 0);
    }
}
