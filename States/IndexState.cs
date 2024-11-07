using ChroniclesExporter.Internal.StateMachine;
using ChroniclesExporter.Database;
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
        DbHandler.Load();
        string[] files = Directory.GetFiles(FileUtility.GetDataRoot(), "*.md", SearchOption.AllDirectories);
        foreach (string file in files)
        {
            if (FileUtility.TryGetTypeFromPath(file, out ETable table))
            {
                TableHandler.Register(file, table);
            }
        }
        
        DrawHeader();
        DrawIndexBreakdown(files.Length);
        DrawIndexTable();
        
        StateMachine.Goto(EProgramState.MdRead);
    }

    private static void DrawHeader()
    {
        Rule header = new Rule("[blue]Indexing[/]")
        {
            Justification = Justify.Left
        };
        AnsiConsole.Write(header);
    }
    
    private static void DrawIndexBreakdown(int pFileCount)
    {
        BreakdownChart chart = new BreakdownChart();
        chart.AddItem("Indexed", TableHandler.Count, Color.Green);
        chart.AddItem("Un-Indexed", pFileCount - TableHandler.Count, Color.Grey);
        chart.Width(32);
        AnsiConsole.Write(new Panel(chart));
    }
    
    private static void DrawIndexTable()
    {
        Spectre.Console.Table table = new Spectre.Console.Table();
        table.AddColumn("Type");
        table.AddColumn("Indexed Amount");
        table.AddRow("Settings", SettingsHandler.Count.ToString());
        table.AddRow("Table Writers", DbHandler.TableCount.ToString());
        table.AddRow("Link Writers", DbHandler.LinkCount.ToString());
        table.Border(TableBorder.Rounded);
        table.Width(36);
        AnsiConsole.Write(table);
    }
}
