using ChroniclesExporter.IO;

namespace ChroniclesExporter.Tables.Languages;

public class ScriptReader : MdReader<Script>
{
    protected override bool TryGetProperties(string pLine, ref Script pData) => false;
}
