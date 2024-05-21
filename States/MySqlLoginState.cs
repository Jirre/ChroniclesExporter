using ChroniclesExporter.Internal.StateMachine;
using ChroniclesExporter.MySql;
using ChroniclesExporter.StateMachine;
using Spectre.Console;

namespace ChroniclesExporter.States;

public class MySqlLoginState(StateMachine<EProgramState> pStateMachine, EProgramState pId) : 
    StateBehaviour<EProgramState>(pStateMachine, pId)
{
    public override void Update()
    {
        Console.Clear();
        DrawHeader();
        Console.CursorVisible = true;
        MySqlHandler.UserId = AnsiConsole.Ask<string>("User Id:");
        MySqlHandler.Password = AnsiConsole.Prompt(new TextPrompt<string>("Password:").Secret());
        Console.CursorVisible = false;
        
        StateMachine.Goto(EProgramState.MySqlTest);
    }

    private static void DrawHeader()
    {
        Rule header = new Rule("[blue]MySql Login Credential[/]")
        {
            Justification = Justify.Left
        };
        AnsiConsole.Write(header);
    }
}
