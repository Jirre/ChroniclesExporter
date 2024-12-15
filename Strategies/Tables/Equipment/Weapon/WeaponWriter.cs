using ChroniclesExporter.IO.Database;
using Npgsql;

namespace ChroniclesExporter.Tables.Equipment.Weapon;

public class WeaponWriter : DbTableWriter<Weapon>
{
    protected override ETable TableId => ETable.Weapon;
    protected override string TableName => "weapons";

    protected override string[] Fields => ["id", "name", "content", 
        "proficiency", "grip", "categories", 
        "damage_type", "damage", "range", 
        "cost", "weight"];
    protected override async Task ImportRow(NpgsqlBinaryImporter pImporter, Weapon pData)
    {
        await pImporter.WriteAsync(pData.Id.ToByteArray(true));
        await pImporter.WriteAsync(pData.Name);
        await pImporter.WriteAsync(pData.Content);
        
        await pImporter.WriteAsync(pData.Proficiency);
        await pImporter.WriteAsync(pData.Grip);
        await pImporter.WriteAsync(pData.Categories);
        
        await pImporter.WriteAsync(pData.DamageType);
        await pImporter.WriteAsync(pData.Damage);
        await pImporter.WriteAsync(pData.Range);
        
        await pImporter.WriteAsync(pData.Cost);
        await pImporter.WriteAsync(pData.Weight);
    }
}
