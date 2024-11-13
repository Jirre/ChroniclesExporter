using ChroniclesExporter.IO.Database;
using Npgsql;

namespace ChroniclesExporter.Strategy.Actions;

public class ActionWriter : DbTableWriter<Action>
{
    protected override ETable TableId => ETable.Actions;
    protected override string TableName => "actions";
    protected override string[] Fields => new[] {"id", "name", "details", "type", "content"};

    protected override async Task ImportRow(NpgsqlBinaryImporter pImporter, Action pData)
    {
        await pImporter.WriteAsync(pData.Id.ToByteArray(true));
        await pImporter.WriteAsync(pData.Name);
        await pImporter.WriteAsync(pData.Details);
        await pImporter.WriteAsync(pData.Type);
        await pImporter.WriteAsync(pData.Content);
    }
}
