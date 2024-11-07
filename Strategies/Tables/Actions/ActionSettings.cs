using ChroniclesExporter.Settings;

namespace ChroniclesExporter.Strategy.Actions;

[Settings(ETable.Actions)]
public class ActionSettings : ISettings<Action, ActionReader, ActionWriter>
{
    public string FilePath => "Actions";

    public string Url(Action pAction)
    {
        return "/Actions?id={0}";
    }

    public string LinkClasses(Action pAction)
    {
        return "link-action tooltip tooltip-action";
    }

    public string LinkIcon(Action pAction)
    {
        return pAction.Type switch
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
