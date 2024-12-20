﻿using ChroniclesExporter.Settings;
using ChroniclesExporter.Table;

namespace ChroniclesExporter.Strategy.Traits;

[Settings(ETable.Traits)]
public class TraitSettings : ISettings<TraitReader, TraitWriter>
{
    public string FilePath => "Properties/Traits";

    public string Url(IRow pData)
    {
        return "/traits/{1}";
    }

    public string LinkIcon(IRow pData)
    {
        return "trait";
    }

    public ETable[] Dependencies => [];
}
