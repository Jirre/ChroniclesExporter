using ChroniclesExporter.Settings;
using ChroniclesExporter.Table;

namespace ChroniclesExporter.Tables.Features;

[Settings(ETable.Features)]
public class FeatureSettings : ISettings<FeatureReader, FeatureWriter>
{
    public string FilePath => "Features";
    public string Url(IRow pData) => "";
    public string LinkIcon(IRow pData) => "";

    public ETable[] Dependencies => [];
}
