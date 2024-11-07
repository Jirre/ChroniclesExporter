using ChroniclesExporter.IO.Database;
using Npgsql;

namespace ChroniclesExporter.Strategy.TraitCategories;

public class TraitCategoryWriter : DbTableWriter<TraitCategory>
{
    public override ETable TableId => ETable.TraitCategories;
    protected override string TableName => "traitcategories";
    protected override string[] Fields => new[] {"id", "name"};

    protected override async Task ImportRow(NpgsqlBinaryImporter pImporter, TraitCategory pData)
    {
        await pImporter.WriteAsync(pData.Id.ToByteArray(true));
        await pImporter.WriteAsync(pData.Name);
    }
}
