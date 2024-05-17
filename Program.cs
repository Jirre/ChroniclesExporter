using ChroniclesExporter.StateMachine;
using ChroniclesExporter.States;

namespace ChroniclesExporter;

public static class Program
{
    private static StateMachine<EProgramState> _stateMachine = new StateMachine<EProgramState>();
    
    private static void Main(string[] args)
    {
        InitStateMachine();
        _stateMachine.Goto(EProgramState.Init);

        while (_stateMachine.IsRunning())
        {
            _stateMachine.Update();
        }

        Console.ReadKey();
    }

    private static void InitStateMachine()
    {
        _stateMachine.Add(new InitState(_stateMachine, EProgramState.Init));
        _stateMachine.Add(new MySqlLoginState(_stateMachine, EProgramState.MySqlLogin));
    }
}
