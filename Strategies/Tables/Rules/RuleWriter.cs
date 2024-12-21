using ChroniclesExporter.IO.Database;
using Npgsql;

namespace ChroniclesExporter.Tables.Rules;

public class RuleWriter : DbTableWriter<Rule>
{
    protected override ETable TableId => ETable.Rules;
    protected override string TableName => "rules";
    protected override string[] Fields => ["id", "name", "content", "category", "description", "priority"];
    protected override async Task ImportRow(NpgsqlBinaryImporter pImporter, Rule pData)
    {
        await pImporter.WriteAsync(pData.Id.ToByteArray(true));
        await pImporter.WriteAsync(pData.Name);
        await pImporter.WriteAsync(pData.Content);
        await pImporter.WriteAsync(pData.Category);
        await pImporter.WriteAsync(pData.Description);
        await pImporter.WriteAsync(pData.Priority);
    }
}
