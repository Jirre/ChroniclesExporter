using ChroniclesExporter.Internal.StateMachine;
using ChroniclesExporter.IO;
using ChroniclesExporter.Log;
using ChroniclesExporter.Settings;
using ChroniclesExporter.StateMachine;
using ChroniclesExporter.Table;
using ChroniclesExporter.Utility;

namespace ChroniclesExporter.States;

public class MdReadState(StateMachine<EProgramState> pStateMachine, EProgramState pId) : 
    StateBehaviour<EProgramState>(pStateMachine, pId)
{
    private Task? _task;

    public override void Activate()
    {
        base.Activate();
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

            if (Activator.CreateInstance(settings.Reader) is IReader reader)
                tasks.Add(reader.Read(kvp.Value.ToArray()));
        }

        _task = Task.WhenAll(tasks);
        ConsoleUtility.WriteMarkedLine("Reading Markdown Data", EConsoleMark.Waiting);
    }

    public override void Update()
    {
        if (_task == null)
        {
            ConsoleUtility.OverwriteMarkedLine("Read Task Lost", EConsoleMark.Error);
            LogHandler.Error(ELogCode.TaskNotFound, "Read Task Lost");
            StateMachine.Goto(EProgramState.Log);
            return;
        }

        if (!_task.IsCompleted)
        {
            ConsoleUtility.OverwriteMarkedLine("Reading Markdown Data", EConsoleMark.Waiting);
            return;
        }

        if (_task.IsFaulted)
        {
            ConsoleUtility.OverwriteMarkedLine("Markdown Read Faulted", EConsoleMark.Error);
            LogHandler.Error(ELogCode.MdReaderError, "Read Task Faulted");
            StateMachine.Goto(EProgramState.Log);
            return;
        }
        if (_task.IsCanceled)
        {
            ConsoleUtility.OverwriteMarkedLine("Markdown Read Canceled", EConsoleMark.Error);
            LogHandler.Error(ELogCode.MdReaderError, "Read Task Canceled");
            StateMachine.Goto(EProgramState.Log);
        }
        
        ConsoleUtility.OverwriteMarkedLine("Reading Completed", EConsoleMark.Check);
        Console.WriteLine();
        StateMachine.Goto(EProgramState.MySqlWrite);
    }
}
