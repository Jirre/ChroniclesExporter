using ChroniclesExporter.IO.Database;
using ChroniclesExporter.Table;
using Npgsql;

namespace ChroniclesExporter.Strategy.Traits;

// ReSharper disable once InconsistentNaming
public class Trait_Categories : DbLinkWriter
{
    protected override ELink LinkId => ELink.TraitCategories;
    protected override string TableName => "traits_categories";
    protected override string[] Fields => new[] {"trait_id", "category"};
    protected override async Task ImportRow(NpgsqlBinaryImporter pImporter, ILink pData)
    {
        await pImporter.WriteAsync(pData.Source.ToByteArray(true));
        await pImporter.WriteAsync(pData.Target);
    }
}

public class CategoryLink(Guid pSource, ETraitCategories pTarget) : ILink
{
    public Guid Source { get; } = pSource;
    public object Target { get; } = pTarget;
}