using ChroniclesExporter.Settings;
using ChroniclesExporter.Table;

namespace ChroniclesExporter.Tables.Languages;

[Settings(ETable.LanguageScripts)]
public class ScriptSettings : ISettings<ScriptReader, ScriptWriter>
{
    public string FilePath => "Languages/Language_Scripts";
    public string Url(IRow pData) => "/languages?id={0}";

    public string LinkIcon(IRow pData) => "language-script";

    public ETable[] Dependencies => [];
}
