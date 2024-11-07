using ChroniclesExporter.Database;
using ChroniclesExporter.Internal.StateMachine;
using ChroniclesExporter.StateMachine;
using ChroniclesExporter.Utility;
using Npgsql;
using Spectre.Console;

namespace ChroniclesExporter.States;

public class DbTestState(StateMachine<EProgramState> pStateMachine, EProgramState pId) :
    StateBehaviour<EProgramState>(pStateMachine, pId)
{
    private int _outputCode;
    private Task? _task;

    public override void Activate()
    {
        base.Activate();
        _task = TestConnectionAsync();

        AnsiConsole.Status().Start("Testing Database Connection", pContext =>
        {
            pContext.Spinner(Spinner.Known.Dots);
            pContext.SpinnerStyle(Style.Parse("blue"));

            while (!_task.IsCompleted)
            {
            }
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
            ? EProgramState.DbLogin
            : EProgramState.Log);
    }

    private string GetOutputContext()
    {
        return _outputCode switch
        {
            0 => "Database Test Successful",
            1042 => "Database Test Failed: Connection failed",
            1045 => "Database Test Failed: Login Credentials invalid",
            1049 => "Database Test Failed: Database credentials invalid",
            _ => $"Uncaught Database Error({_outputCode})"
        };
    }

    private async Task TestConnectionAsync()
    {
        DbHandler.Setup();

        try
        {
            NpgsqlConnection connection = await DbHandler.DataSource.OpenConnectionAsync();
            await connection.OpenAsync();
        }
        catch (NpgsqlException e)
        {
            _outputCode = e.ErrorCode;
        }
    }
}
