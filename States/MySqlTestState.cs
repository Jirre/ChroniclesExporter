using ChroniclesExporter.Internal.StateMachine;
using ChroniclesExporter.MySql;
using ChroniclesExporter.StateMachine;
using ChroniclesExporter.Utility;
using MySqlConnector;
using Spectre.Console;

namespace ChroniclesExporter.States;

public class MySqlTestState(StateMachine<EProgramState> pStateMachine, EProgramState pId) : 
    StateBehaviour<EProgramState>(pStateMachine, pId)
{
    private Task? _task;
    private int _outputCode;

    public override void Activate()
    {
        base.Activate();
        _task = TestConnectionAsync();

        AnsiConsole.Status().Start("Testing MySql Connection", pContext =>
        {
            pContext.Spinner(Spinner.Known.Dots);
            pContext.SpinnerStyle(Style.Parse("blue"));

            while (!_task.IsCompleted) {}
        });
    }

    public override void Update()
    {
        if (!_task?.IsCompleted ?? true)
            return;

        Console.WriteLine();
        ConsoleUtility.WriteMarkedLine(GetOutputContext(), _outputCode == 0 ? EConsoleMark.Check : EConsoleMark.Error);
        Console.WriteLine();
        if (_outputCode == 0)
        {
            StateMachine.Goto(EProgramState.Index);
            return;
        }
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
        await using MySqlConnection connection = new($"server={MySqlHandler.Server};" +
                                                     $"port={MySqlHandler.Port};" +
                                                     $"database={MySqlHandler.Database};" +
                                                     $"user={MySqlHandler.UserId};" +
                                                     $"password={MySqlHandler.Password}");

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
