using ChroniclesExporter.Internal.StateMachine;
using ChroniclesExporter.MySql;
using ChroniclesExporter.StateMachine;

namespace ChroniclesExporter.States;

public class MySqlLoginState(StateMachine<EProgramState> pStateMachine, EProgramState pId) : 
    StateBehaviour<EProgramState>(pStateMachine, pId)
{
    public override void Update()
    {
        Console.CursorVisible = true;
        Console.WriteLine("--- MySql Login Credentials ---");
        Console.Write("User Id: ");
        MySqlHandler.UserId = Console.ReadLine() ?? string.Empty;
        Console.Write("Password: ");
        MySqlHandler.Password = ReadPassword();
        Console.WriteLine();
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
