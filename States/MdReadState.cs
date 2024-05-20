using ChroniclesExporter.Internal.StateMachine;
using ChroniclesExporter.IO;
using ChroniclesExporter.Log;
using ChroniclesExporter.Settings;
using ChroniclesExporter.StateMachine;
using ChroniclesExporter.Table;
using ChroniclesExporter.Utility;
using Spectre.Console;

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
        Dictionary<ETable, IReader> readers = new Dictionary<ETable, IReader>();
        foreach (KeyValuePair<ETable, List<string>> kvp in files)
        {
            if (!SettingsHandler.TryGetSettings(kvp.Key, out ISettings settings))
                continue;

            if (Activator.CreateInstance(settings.Reader) is IReader reader)
            {
                readers.Add(kvp.Key, reader);
                tasks.Add(reader.Read(kvp.Value.ToArray()));
            }
        }
        _task = Task.WhenAll(tasks);
        
        Rule header = new Rule("[blue]Reading[/]");
        header.Justification = Justify.Left;
        AnsiConsole.Write(header);
        AnsiConsole.Progress()
            .Start(ctx =>
            {
                Dictionary<ETable, ProgressTask> tasks = new Dictionary<ETable, ProgressTask>();
                foreach (KeyValuePair<ETable, IReader> kvp in readers)
                {
                    ProgressTask task = ctx.AddTask(kvp.Key.ToString(), true, files[kvp.Key].Count);
                    tasks.Add(kvp.Key, task);
                }

                while (!_task.IsCompleted)
                {
                    foreach (KeyValuePair<ETable, IReader> kvp in readers)
                    {
                        tasks[kvp.Key].Value(kvp.Value.Progress);
                    }
                }
                if (!_task.IsCompletedSuccessfully)
                    return;
                
                foreach (KeyValuePair<ETable, IReader> kvp in readers)
                {
                    // Force it to 100%
                    tasks[kvp.Key].Value(files[kvp.Key].Count);
                }
            });
    }

    public override void Update()
    {
        if (_task == null)
        {
            ConsoleUtility.WriteMarkedLine("Read Task Lost", EConsoleMark.Error);
            LogHandler.Error(ELogCode.TaskNotFound, "Read Task Lost");
            StateMachine.Goto(EProgramState.Log);
            return;
        }

        if (!_task.IsCompleted)
            return;

        if (_task.IsFaulted)
        {
            ConsoleUtility.WriteMarkedLine("Markdown Read Faulted", EConsoleMark.Error);
            LogHandler.Error(ELogCode.MdReaderError, "Read Task Faulted");
            StateMachine.Goto(EProgramState.Log);
            return;
        }
        if (_task.IsCanceled)
        {
            ConsoleUtility.WriteMarkedLine("Markdown Read Canceled", EConsoleMark.Error);
            LogHandler.Error(ELogCode.MdReaderError, "Read Task Canceled");
            StateMachine.Goto(EProgramState.Log);
        }
        
        ConsoleUtility.WriteMarkedLine("Reading Completed", EConsoleMark.Check);
        Console.WriteLine();
        StateMachine.Goto(EProgramState.MySqlWrite);
    }
}
