using ChroniclesExporter.Internal.StateMachine;
using ChroniclesExporter.MySql;
using ChroniclesExporter.StateMachine;
using ChroniclesExporter.Utility;
using MySqlConnector;

namespace ChroniclesExporter.States;

public class MySqlTestState(StateMachine<EProgramState> pStateMachine, EProgramState pId) : 
    StateBehaviour<EProgramState>(pStateMachine, pId)
{
    private Task _task;
    private int _outputCode = 0;

    public override void Activate()
    {
        base.Activate();
        ConsoleUtility.WriteMarkedLine("Testing MySql Connection", EConsoleMark.Waiting);
        _task = TestConnectionAsync();
    }

    public override void Update()
    {
        if (!_task.IsCompleted)
        {
            ConsoleUtility.OverwriteMarkedLine("Testing MySql Connection", EConsoleMark.Waiting);
            return;
        }
        ConsoleUtility.OverwriteMarkedLine(GetOutputContext(), _outputCode == 0 ? EConsoleMark.Check : EConsoleMark.Error);
        if (_outputCode == 0)
        {
            StateMachine.Goto(EProgramState.Index);
            return;
        }
        Console.WriteLine();
        Console.CursorVisible = true;
        StateMachine.Goto(ConsoleUtility.ConfirmPrompt("Change Login Credentials?")
            ? EProgramState.MySqlLogin
            : EProgramState.Log);
    }

    private string GetOutputContext()
    {
        return _outputCode switch
        {
            0 => "MySql Test Successful",
            1042 => "MySql Test Failed: Connection failed",
            1045 => "MySql Test Failed: Login Credentials invalid",
            1049 => "MySql Test Failed: Database credentials invalid",
            _ => $"Uncaught MySql Error({_outputCode})"
        };
    }

    private async Task TestConnectionAsync()
    {
        await using MySqlConnection connection = new($"server={MySqlHandler.SERVER};" +
                                                     $"port={MySqlHandler.PORT};" +
                                                     $"database={MySqlHandler.DATABASE};" +
                                                     $"user={MySqlHandler.USER_ID};" +
                                                     $"password={MySqlHandler.PASSWORD}");

        try
        {
            await connection.OpenAsync();
        }
        catch (MySqlException e)
        {
            _outputCode = e.Number;
        }
    }
}
