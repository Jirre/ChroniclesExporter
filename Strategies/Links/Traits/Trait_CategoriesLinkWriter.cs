using ChroniclesExporter.IO.Database;
using ChroniclesExporter.Strategy.Links;

namespace ChroniclesExporter.Strategy.Traits;

// ReSharper disable once InconsistentNaming
public class Trait_CategoriesLinkWriter : DbLinkWriter<Link>
{
    public override ELink LinkId => ELink.TraitCategories;

    protected override string TableName => "traits_categories";
    protected override string[] Fields => new[] {"trait_id", "category_id"};
}
