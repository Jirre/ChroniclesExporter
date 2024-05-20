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
        Rule header = new Rule("[blue]MySql Login Credential[/]");
        header.Justification = Justify.Left;
        AnsiConsole.Write(header);
        Console.CursorVisible = true;
        MySqlHandler.UserId = AnsiConsole.Ask<string>("User Id:");
        MySqlHandler.Password = AnsiConsole.Prompt(new TextPrompt<string>("Password:").Secret());
        Console.CursorVisible = false;
        
        StateMachine.Goto(EProgramState.MySqlTest);
    }
    
    private static string ReadPassword()
    {
        string password = string.Empty;
        ConsoleKeyInfo key;

        do
        {
            key = Console.ReadKey(true);

            // Handle backspace
            if (key.Key == ConsoleKey.Backspace && password.Length > 0)
            {
                password = password[0..^1];
                Console.Write("\b \b");
            }
            // Ignore other special keys
            else if (!char.IsControl(key.KeyChar))
            {
                password += key.KeyChar;
                Console.Write("*");
            }
        }
        while (key.Key != ConsoleKey.Enter);

        return password;
    }
}
