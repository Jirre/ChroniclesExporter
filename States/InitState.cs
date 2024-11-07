using ChroniclesExporter.Database;
using ChroniclesExporter.Internal.StateMachine;
using ChroniclesExporter.StateMachine;
using ChroniclesExporter.Utility;

namespace ChroniclesExporter.States;

public class InitState(StateMachine<EProgramState> pStateMachine, EProgramState pId) :
    StateBehaviour<EProgramState>(pStateMachine, pId)
{
    public override void Update()
    {
        ReadEnvFiles();
        DbHandler.SetEnvironmentVariables();

        if (string.IsNullOrWhiteSpace(DbHandler.Username) || string.IsNullOrWhiteSpace(DbHandler.Password))
        {
            StateMachine.Goto(EProgramState.DbLogin);
            return;
        }

        StateMachine.Goto(EProgramState.DbTest);
    }

    private static void ReadEnvFiles()
    {
        string[] files = Directory.GetFiles(FileUtility.GetRoot(), "*.env");
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
