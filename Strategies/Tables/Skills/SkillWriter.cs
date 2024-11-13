using ChroniclesExporter.IO.Database;
using Npgsql;

namespace ChroniclesExporter.Strategy.Traits;

public class SkillWriter : DbTableWriter<Skill>
{
    protected override ETable TableId => ETable.Skills;
    protected override string TableName => "skills";
    protected override string[] Fields => new[] {"id", "name", "ability", "content"};

    protected override async Task ImportRow(NpgsqlBinaryImporter pImporter, Skill pData)
    {
        await pImporter.WriteAsync(pData.Id.ToByteArray(true));
        await pImporter.WriteAsync(pData.Name);
        await pImporter.WriteAsync(pData.Ability);
        await pImporter.WriteAsync(pData.Content);
    }
}
