using ChroniclesExporter.IO;
using ChroniclesExporter.IO.MySql;
using ChroniclesExporter.Log;
using ChroniclesExporter.MySql;
using ChroniclesExporter.StateMachine;
using ChroniclesExporter.Table;
using Spectre.Console;

namespace ChroniclesExporter.States;

public class MySqlLinkState(StateMachine<EProgramState> pStateMachine, EProgramState pId) : 
    AProgressState<ELink, IWriter>(pStateMachine, pId)
{
    protected override EProgramState DefaultCompleteState => EProgramState.Log;
    protected override ELogCode DefaultErrorCode => ELogCode.MySqlError;
    
    protected override List<Task> BuildHandlers()
    {
        foreach (KeyValuePair<ELink, List<ILink>> kvp in TableHandler.Links)
        {
            if (!MySqlHandler.TryGetWriter(kvp.Key, out MySqlWriter<ILink> writer)) continue;
            Handlers.Add(kvp.Key, writer);
            writer.Prepare(kvp.Value.ToArray());
        }
        
        List<Task> tasks = new List<Task>();
        foreach (KeyValuePair<ELink, IWriter> kvp in Handlers)
        {
            tasks.Add(kvp.Value.Write());
        }

        return tasks;
    }
    
    protected override void OnHeaderDraw()
    {
        Rule header = new Rule("[blue]Writing Link Tables to MySql Database[/]")
        {
            Justification = Justify.Left
        };
        AnsiConsole.Write(header);
    }
}
