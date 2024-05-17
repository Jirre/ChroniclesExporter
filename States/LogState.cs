using ChroniclesExporter.Internal.StateMachine;
using ChroniclesExporter.Log;
using ChroniclesExporter.StateMachine;
using ChroniclesExporter.Utility;

namespace ChroniclesExporter.States;

public class LogState(StateMachine<EProgramState> pStateMachine, EProgramState pId) : StateBehaviour<EProgramState>(pStateMachine, pId)
{
    public override void Update()
    {
        EConsoleMark logs = 0;
        if (LogHandler.InfoCount > 0) logs |= EConsoleMark.Info;
        if (LogHandler.WarningCount > 0) logs |= EConsoleMark.Warning;
        if (LogHandler.ErrorCount > 0) logs |= EConsoleMark.Error;

        if (logs == 0)
        {
            StateMachine.Goto(EProgramState.Complete);
            return;
        }

        Console.WriteLine("--- Log Output ---");
        Console.WriteLine($"{LogHandler.InfoCount} Info Log{(LogHandler.InfoCount != 1 ? 's' : ' ')}");
        Console.WriteLine($"{LogHandler.WarningCount} Warning Log{(LogHandler.WarningCount != 1 ? 's' : ' ')}");
        Console.WriteLine($"{LogHandler.ErrorCount} Error Log{(LogHandler.ErrorCount != 1 ? 's' : ' ')}");
        
        int output = ConsoleUtility.ComplexPrompt("Print:", [
            "None",
            "Info",
            "Warning",
            "Error",
            "Warning + Error",
            "All"
        ]);

        EConsoleMark outputMarks = OutputToMark(output);
        if (outputMarks != 0)
            LogHandler.Print(outputMarks);
        
        StateMachine.Goto(EProgramState.Complete);
    }

    private static EConsoleMark OutputToMark(int pOption)
    {
        return pOption switch
        {
            1 => EConsoleMark.Info,
            2 => EConsoleMark.Warning,
            3 => EConsoleMark.Error,
            4 => EConsoleMark.Warning | EConsoleMark.Error,
            5 => EConsoleMark.Info | EConsoleMark.Warning | EConsoleMark.Error,
            _ => 0
        };
    }
}
