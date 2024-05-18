using ChroniclesExporter.Internal.StateMachine;
using ChroniclesExporter.MySql;
using ChroniclesExporter.Settings;
using ChroniclesExporter.StateMachine;
using ChroniclesExporter.Table;
using ChroniclesExporter.Utility;

namespace ChroniclesExporter.States;

public class IndexState(StateMachine<EProgramState> pStateMachine, EProgramState pId) : 
    StateBehaviour<EProgramState>(pStateMachine, pId)
{
    public override void Update()
    {
        SettingsHandler.Load();
        MySqlHandler.Load();
        string[] files = Directory.GetFiles(IOUtility.GetDataRoot(), "*.md", SearchOption.AllDirectories);
        foreach (string file in files)
        {
            if (SettingsHandler.TryGetTable(file, out ETable table))
            {
                TableHandler.Register(file, table);
            }
        }
        Console.WriteLine("--- Indexing ---");
        ConsoleUtility.WriteMarkedLine($"{files.Length} Files Found, {TableHandler.Count} files indexed", 
            files.Length == TableHandler.Count ? EConsoleMark.Check : EConsoleMark.Warning);
        ConsoleUtility.WriteMarkedLine($"{MySqlHandler.TableCount} MySql Table Writers Indexed", EConsoleMark.Check);
        ConsoleUtility.WriteMarkedLine($"{MySqlHandler.LinkCount} MySql Link Writers Indexed", EConsoleMark.Check);
        ConsoleUtility.WriteMarkedLine($"{SettingsHandler.Count} Settings Indexed", EConsoleMark.Check);
        Console.WriteLine();
        
        StateMachine.Goto(EProgramState.MdRead);
    }
}
