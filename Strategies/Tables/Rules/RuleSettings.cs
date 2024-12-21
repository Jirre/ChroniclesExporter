using ChroniclesExporter.Settings;
using ChroniclesExporter.Table;

namespace ChroniclesExporter.Tables.Rules;

[Settings(ETable.Rules)]
public class RuleSettings : ISettings<RuleReader, RuleWriter>
{
    public string FilePath => "Rules";
    public ETable[] Dependencies => [];

    public string Url(IRow pData) => "/rules/{0}";
    public string LinkIcon(IRow pData) => "rule";
}
