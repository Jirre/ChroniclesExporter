using ChroniclesExporter.Internal.StateMachine;
using ChroniclesExporter.MySql;
using ChroniclesExporter.Settings;
using ChroniclesExporter.StateMachine;
using ChroniclesExporter.Table;
using ChroniclesExporter.Utility;
using Spectre.Console;

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
            if (IOUtility.TryGetTypeFromPath(file, out ETable table))
            {
                TableHandler.Register(file, table);
            }
        }

        Rule header = new Rule("[blue]Indexing[/]");
        header.Justification = Justify.Left;
        AnsiConsole.Write(header);
        
        BreakdownChart chart = new BreakdownChart();
        chart.AddItem("Indexed", TableHandler.Count, Color.Green);
        chart.AddItem("Un-Indexed", files.Length - TableHandler.Count, Color.Grey);
        chart.Width(32);
        AnsiConsole.Write(new Panel(chart));
        
        Spectre.Console.Table consoleTable = new Spectre.Console.Table();
        consoleTable.AddColumn("Type");
        consoleTable.AddColumn("Indexed Amount");
        consoleTable.AddRow("Settings", SettingsHandler.Count.ToString());
        consoleTable.AddRow("Table Writers", MySqlHandler.TableCount.ToString());
        consoleTable.AddRow("Link Writers", MySqlHandler.LinkCount.ToString());
        consoleTable.Border(TableBorder.Rounded);
        consoleTable.Width(36);
        AnsiConsole.Write(consoleTable);
        
        StateMachine.Goto(EProgramState.MdRead);
    }
}
