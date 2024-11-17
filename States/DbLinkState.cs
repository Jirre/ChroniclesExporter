using ChroniclesExporter.Database;
using ChroniclesExporter.IO;
using ChroniclesExporter.IO.Database;
using ChroniclesExporter.Log;
using ChroniclesExporter.StateMachine;
using ChroniclesExporter.Table;
using Spectre.Console;

namespace ChroniclesExporter.States;

public class DbLinkState(StateMachine<EProgramState> pStateMachine, EProgramState pId) :
    AProgressState<ELink, IWriter>(pStateMachine, pId)
{
    protected override EProgramState DefaultCompleteState => EProgramState.Log;
    protected override ELogCode DefaultErrorCode => ELogCode.DatabaseError;

    protected override List<Task> BuildHandlers()
    {
        foreach (KeyValuePair<ELink, List<ILink>> kvp in TableHandler.Links)
        {
            if (!DbHandler.TryGetWriter(kvp.Key, out DbWriter<ILink> writer)) continue;
            Handlers.Add(kvp.Key, writer);
            writer.Prepare(kvp.Value.ToArray());
        }

        List<Task> tasks = new();
        foreach (KeyValuePair<ELink, IWriter> kvp in Handlers) tasks.Add(kvp.Value.Write());

        return tasks;
    }

    protected override void OnHeaderDraw()
    {
        Rule header = new("[blue]Writing Link Tables to Database[/]")
        {
            Justification = Justify.Left
        };
        AnsiConsole.Write(header);
    }
}
