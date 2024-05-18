using ChroniclesExporter.Internal.StateMachine;
using ChroniclesExporter.MySql;
using ChroniclesExporter.StateMachine;
using ChroniclesExporter.Utility;

namespace ChroniclesExporter.States;

public class InitState(StateMachine<EProgramState> pStateMachine, EProgramState pId) : 
    StateBehaviour<EProgramState>(pStateMachine, pId)
{
    public override void Update()
    {
        ReadEnvFiles();
        MySqlHandler.SetEnvironmentVariables();
        
        if (string.IsNullOrWhiteSpace(MySqlHandler.UserId) || string.IsNullOrWhiteSpace(MySqlHandler.Password))
        {
            StateMachine.Goto(EProgramState.MySqlLogin);
            return;
        }
        StateMachine.Goto(EProgramState.MySqlTest);
    }

    private static void ReadEnvFiles()
    {
        string[] files = Directory.GetFiles(IOUtility.GetRoot(), "*.env");
        if (files.Length <= 0) return;
        
        foreach (string file in files)
        {
            if (!File.Exists(file))
                continue;

            foreach (string line in File.ReadAllLines(file))
            {
                string[] parts = line.Split('=', StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length != 2)
                    continue;
                    
                Environment.SetEnvironmentVariable(parts[0], parts[1]);
            }
        }
    }
}
