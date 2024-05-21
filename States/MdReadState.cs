using ChroniclesExporter.IO;
using ChroniclesExporter.Log;
using ChroniclesExporter.Settings;
using ChroniclesExporter.StateMachine;
using ChroniclesExporter.Table;
using Spectre.Console;

namespace ChroniclesExporter.States;

public class MdReadState(StateMachine<EProgramState> pStateMachine, EProgramState pId) : 
    AProgressState<ETable, IReader>(pStateMachine, pId)
{
    protected override EProgramState DefaultCompleteState => EProgramState.MySqlWrite;
    protected override ELogCode DefaultErrorCode => ELogCode.MdReaderError;
    
    protected override List<Task> BuildHandlers()
    {
        Dictionary<ETable, List<string>> files = new Dictionary<ETable, List<string>>();
        foreach (TableEntry table in TableHandler.Entries)
        {
            if (!files.ContainsKey(table.Id))
                files.Add(table.Id, new List<string>());
            
            files[table.Id].Add(table.Path);
        }
        
        List<Task> tasks = new List<Task>();
        foreach (KeyValuePair<ETable, List<string>> kvp in files)
        {
            if (!SettingsHandler.TryGetSettings(kvp.Key, out ISettings settings))
                continue;

            if (Activator.CreateInstance(settings.Reader) is not IReader reader) continue;
            Handlers.Add(kvp.Key, reader);
            tasks.Add(reader.Read(kvp.Value.ToArray()));
        }

        return tasks;
    }
    
    protected override void OnHeaderDraw()
    {
        Rule header = new Rule("[blue]Reading Markdown Files[/]")
        {
            Justification = Justify.Left
        };
        AnsiConsole.Write(header);
    }
}
