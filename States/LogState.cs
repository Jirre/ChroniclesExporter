using ChroniclesExporter.Internal.StateMachine;
using ChroniclesExporter.Log;
using ChroniclesExporter.StateMachine;
using ChroniclesExporter.Utility;
using Spectre.Console;

namespace ChroniclesExporter.States;

public class LogState(StateMachine<EProgramState> pStateMachine, EProgramState pId) : StateBehaviour<EProgramState>(pStateMachine, pId)
{
    public override void Update()
    {
        EConsoleMark logs = 0;
        if (LogHandler.InfoCount > 0) logs |= EConsoleMark.Info;
        if (LogHandler.WarningCount > 0) logs |= EConsoleMark.Warning;
        if (LogHandler.ErrorCount > 0) logs |= EConsoleMark.Error;

        if (logs == 0)
        {
            StateMachine.Goto(EProgramState.Complete);
            return;
        }

        DrawHeader();
        DrawLogBreakdown();

        EConsoleMark marks = DrawLogPrompt();
        if (marks != 0)
            LogHandler.Print(marks);
        
        StateMachine.Goto(EProgramState.Complete);
    }
    
    private static void DrawHeader()
    {
        Rule header = new Rule("[blue]Print Logs[/]")
        {
            Justification = Justify.Left
        };
        AnsiConsole.Write(header);
    }

    private static void DrawLogBreakdown()
    {
        BreakdownChart chart = new BreakdownChart();
        chart.AddItem("Info", LogHandler.InfoCount, Color.Grey);
        chart.AddItem("Warning", LogHandler.WarningCount, Color.Yellow);
        chart.AddItem("Error", LogHandler.ErrorCount, Color.Red);
        chart.Width(56);
        AnsiConsole.Write(new Panel(chart));
    }

    private static EConsoleMark DrawLogPrompt()
    {
        List<string> outputs = AnsiConsole.Prompt(
            new MultiSelectionPrompt<string>()
                .Title("Select what logs to print")
                .NotRequired()
                .InstructionsText("(Press [blue]<space>[/] to toggle a fruit, " +
                                  "[green]<enter>[/] to accept)")
                .AddChoices("Info", "Warning", "Error"));

        EConsoleMark marks = 0;
        if (outputs.Contains("Info")) marks |= EConsoleMark.Info;
        if (outputs.Contains("Warning")) marks |= EConsoleMark.Warning;
        if (outputs.Contains("Error")) marks |= EConsoleMark.Error;

        return marks;
    }
}
