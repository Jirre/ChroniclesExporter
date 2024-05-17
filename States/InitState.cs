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

        if (TryGetEnvironmentVariable("MYSQL_SERVER", out string server)) MySqlHandler.SERVER = server;
        if (TryGetEnvironmentVariable("MYSQL_PORT", out string port)) MySqlHandler.PORT = port;
        if (TryGetEnvironmentVariable("MYSQL_DATABASE", out string database)) MySqlHandler.DATABASE = database;
        if (TryGetEnvironmentVariable("MYSQL_USER_ID", out string userId)) MySqlHandler.USER_ID = userId;
        if (TryGetEnvironmentVariable("MYSQL_PASSWORD", out string password)) MySqlHandler.PASSWORD = password;

        if (string.IsNullOrWhiteSpace(MySqlHandler.USER_ID) || string.IsNullOrWhiteSpace(MySqlHandler.PASSWORD))
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

    private static bool TryGetEnvironmentVariable(string pKey, out string pValue)
    {
        pValue = Environment.GetEnvironmentVariable(pKey);
        return pValue != null;
    }
}
