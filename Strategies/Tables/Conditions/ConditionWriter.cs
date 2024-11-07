using ChroniclesExporter.IO.Database;
using Npgsql;

namespace ChroniclesExporter.Strategy.Conditions;

public class ConditionWriter : DbTableWriter<Condition>
{
    public override ETable TableId => ETable.Conditions;

    protected override string TableName => "conditions";
    protected override string[] Fields => new[] {"id", "name", "content"};

    protected override async Task ImportRow(NpgsqlBinaryImporter pImporter, Condition pData)
    {
        await pImporter.WriteAsync(pData.Id.ToByteArray(true));
        await pImporter.WriteAsync(pData.Name);
        await pImporter.WriteAsync(pData.Content);
    }
}
