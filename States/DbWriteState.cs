using ChroniclesExporter.Database;
using ChroniclesExporter.IO;
using ChroniclesExporter.IO.Database;
using ChroniclesExporter.Log;
using ChroniclesExporter.StateMachine;
using ChroniclesExporter.Table;
using Spectre.Console;

namespace ChroniclesExporter.States;

public class DbWriteState(StateMachine<EProgramState> pStateMachine, EProgramState pId) :
    AProgressState<ETable, IWriter>(pStateMachine, pId)
{
    protected override EProgramState DefaultCompleteState => EProgramState.DbLink;
    protected override ELogCode DefaultErrorCode => ELogCode.DatabaseError;

    protected override List<Task> BuildHandlers()
    {
        Dictionary<ETable, List<IRow>> tables = new();
        foreach (TableEntry entry in TableHandler.Entries)
        {
            if (!tables.ContainsKey(entry.Id))
                tables.Add(entry.Id, new List<IRow>());

            if (entry.Row != null)
                tables[entry.Id].Add(entry.Row);
        }

        foreach (KeyValuePair<ETable, List<IRow>> kvp in tables)
        {
            if (!DbHandler.TryGetWriter(kvp.Key, out DbWriter<IRow> writer)) continue;
            Handlers.Add((ETable) writer.Id, writer);
            writer.Prepare(kvp.Value.ToArray());
        }

        List<Task> tasks = new();
        foreach (KeyValuePair<ETable, IWriter> kvp in Handlers) tasks.Add(kvp.Value.Write());

        return tasks;
    }

    protected override void OnHeaderDraw()
    {
        Rule header = new("[blue]Writing to Database[/]")
        {
            Justification = Justify.Left
        };
        AnsiConsole.Write(header);
    }
}
