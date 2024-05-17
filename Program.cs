using ChroniclesExporter.StateMachine;
using ChroniclesExporter.States;
using ChroniclesExporter.Utility;

namespace ChroniclesExporter;

public static class Program
{
    private static StateMachine<EProgramState> _stateMachine = new StateMachine<EProgramState>();
    
    private static void Main(string[] args)
    {
        InitStateMachine();
        _stateMachine.Goto(EProgramState.Init);

        while (!_stateMachine.IsCurrentState(EProgramState.Complete))
        {
            _stateMachine.Update();
        }

        Console.ReadKey();
    }

    private static void InitStateMachine()
    {
        _stateMachine.Add(new InitState(_stateMachine, EProgramState.Init));
        _stateMachine.Add(new MySqlLoginState(_stateMachine, EProgramState.MySqlLogin));
        
        _stateMachine.Add(new MySqlTestState(_stateMachine, EProgramState.MySqlTest));
        
        _stateMachine.Add(new LogState(_stateMachine, EProgramState.Log));
        _stateMachine.Add(EProgramState.Complete, CompleteState);
    }
    
    private static void CompleteState() { }
}
