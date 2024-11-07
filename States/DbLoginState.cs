using ChroniclesExporter.Internal.StateMachine;
using ChroniclesExporter.Database;
using ChroniclesExporter.StateMachine;
using Spectre.Console;

namespace ChroniclesExporter.States;

public class DbLoginState(StateMachine<EProgramState> pStateMachine, EProgramState pId) : 
    StateBehaviour<EProgramState>(pStateMachine, pId)
{
    public override void Update()
    {
        Console.Clear();
        DrawHeader();
        Console.CursorVisible = true;
        DbHandler.Username = AnsiConsole.Ask<string>("User Id:");
        DbHandler.Password = AnsiConsole.Prompt(new TextPrompt<string>("Password:").Secret());
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
