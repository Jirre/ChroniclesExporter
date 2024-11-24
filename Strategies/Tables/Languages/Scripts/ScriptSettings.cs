using ChroniclesExporter.Settings;
using ChroniclesExporter.Table;

namespace ChroniclesExporter.Tables.Languages;

[Settings(ETable.LanguageScripts)]
public class ScriptSettings : ISettings<Script, ScriptReader, ScriptWriter>
{
    public string FilePath => "Language_Scripts";
    public string Url(IRow pData) => "/Languages?id={0}";

    public string LinkIcon(IRow pData) => "language-script";

    public ETable[] Dependencies => [];
}
