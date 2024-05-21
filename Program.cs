using System.Diagnostics;
using System.Reflection;
using ChroniclesExporter.StateMachine;
using ChroniclesExporter.States;

namespace ChroniclesExporter;

public static class Program
{
    private static readonly StateMachine<EProgramState> STATE_MACHINE = new StateMachine<EProgramState>();
    
    private static void Main()
    {
        Console.WriteLine(FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).ProductVersion);
        InitStateMachine();
        STATE_MACHINE.Goto(EProgramState.Init);

        while (!STATE_MACHINE.IsCurrentState(EProgramState.Complete))
        {
            STATE_MACHINE.Update();
        }

        Console.ReadKey();
    }

    private static void InitStateMachine()
    {
        STATE_MACHINE.Add(new InitState(STATE_MACHINE, EProgramState.Init));
        STATE_MACHINE.Add(new MySqlLoginState(STATE_MACHINE, EProgramState.MySqlLogin));
        STATE_MACHINE.Add(new MySqlTestState(STATE_MACHINE, EProgramState.MySqlTest));
        
        STATE_MACHINE.Add(new IndexState(STATE_MACHINE, EProgramState.Index));
        STATE_MACHINE.Add(new MdReadState(STATE_MACHINE, EProgramState.MdRead));
        STATE_MACHINE.Add(new MySqlWriteState(STATE_MACHINE, EProgramState.MySqlWrite));
        STATE_MACHINE.Add(new MySqlLinkState(STATE_MACHINE, EProgramState.MySqlLink));
        
        STATE_MACHINE.Add(new LogState(STATE_MACHINE, EProgramState.Log));
        STATE_MACHINE.Add(EProgramState.Complete, CompleteState);
    }
    
    private static void CompleteState() { }
}
