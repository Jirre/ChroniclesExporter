using ChroniclesExporter.Settings;
using ChroniclesExporter.Table;

namespace ChroniclesExporter.Strategy.Actions;

[Settings(ETable.Actions)]
public class ActionSettings : ISettings<ActionReader, ActionWriter>
{
    public string FilePath => "Actions";

    public string Url(IRow pAction)
    {
        return "/actions?id={0}";
    }
    
    public string LinkIcon(IRow pAction)
    {
        if (pAction is not Action action)
            return "action";
        
        return action.Type switch
        {
            EActionTypes.Action => "action",
            EActionTypes.Bonus => "action-bonus",
            EActionTypes.Reaction => "action-reaction",
            EActionTypes.Movement => "action-movement",
            EActionTypes.Free => "action-free",
            _ => throw new ArgumentOutOfRangeException(nameof(pAction), pAction, null)
        };
    }

    public ETable[] Dependencies => [];
}
