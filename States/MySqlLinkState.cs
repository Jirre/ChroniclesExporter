using ChroniclesExporter.Internal.StateMachine;
using ChroniclesExporter.IO.MySql;
using ChroniclesExporter.MySql;
using ChroniclesExporter.StateMachine;
using ChroniclesExporter.Table;
using Spectre.Console;

namespace ChroniclesExporter.States;

public class MySqlLinkState(StateMachine<EProgramState> pStateMachine, EProgramState pId) : 
    StateBehaviour<EProgramState>(pStateMachine, pId)
{
    private readonly List<MySqlWriter<ILink>> _writers = new List<MySqlWriter<ILink>>();

    public override void Activate()
    {
        base.Activate();
        Rule header = new Rule("[blue]MySql Linking[/]");
        header.Justification = Justify.Left;
        AnsiConsole.Write(header);
        foreach (KeyValuePair<ELink,List<ILink>> kvp in TableHandler.Links)
        {
            if (!MySqlHandler.TryGetWriter(kvp.Key, out MySqlWriter<ILink> writer)) continue;
            _writers.Add(writer);
            writer.Prepare(kvp.Value.ToArray());
        }
        
        foreach (MySqlWriter<ILink> writer in _writers)
            writer.Write();

        AnsiConsole.Progress()
            .Columns(
                new TaskDescriptionColumn(),
                new ProgressBarColumn(),
                new PercentageColumn(),
                new SpinnerColumn()
            )
            .Start(ctx =>
            {
                Dictionary<ELink, ProgressTask> progressTasks = new Dictionary<ELink, ProgressTask>();
                foreach (KeyValuePair<ELink, List<ILink>> kvp in TableHandler.Links)
                {
                    progressTasks.Add(kvp.Key, ctx.AddTask(kvp.Key.ToString(), true, kvp.Value.Count));
                }

                bool isReady = false;
                while (!isReady)
                {
                    isReady = true;
                    foreach (MySqlWriter<ILink> writer in _writers)
                    {
                        progressTasks[(ELink) writer.Id].Value(writer.Progress);
                        if (!writer.IsReady)
                            isReady = false;
                    }
                }

                foreach (MySqlWriter<ILink> writer in _writers)
                {
                    progressTasks[(ELink) writer.Id].Value(TableHandler.Links[(ELink) writer.Id].Count);
                }
            });


    }

    public override void Update()
    {
        foreach (MySqlWriter<ILink> writer in _writers)
        {
            if (!writer.IsReady)
                return;
        }
        
        StateMachine.Goto(EProgramState.Log);
    }
}
