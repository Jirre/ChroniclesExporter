using ChroniclesExporter.Internal.StateMachine;
using ChroniclesExporter.IO;
using ChroniclesExporter.Log;
using ChroniclesExporter.StateMachine;
using ChroniclesExporter.Utility;
using Spectre.Console;

namespace ChroniclesExporter.States;

public abstract class AProgressState<TTaskId, TTask>(StateMachine<EProgramState> pStateMachine, EProgramState pId) : 
    StateBehaviour<EProgramState>(pStateMachine, pId) 
    where TTaskId : Enum
    where TTask : ITaskHandler
{
    private Task? _task;
    protected readonly Dictionary<TTaskId, TTask> Handlers = new Dictionary<TTaskId, TTask>();
    
    protected abstract EProgramState DefaultCompleteState { get; }
    protected abstract ELogCode DefaultErrorCode { get; }
    
    public sealed override void Activate()
    {
        base.Activate();
        _task = Task.WhenAll(BuildHandlers());
    }

    protected abstract List<Task> BuildHandlers();

    protected abstract void OnHeaderDraw();
    
    public sealed override void Update()
    {
        OnHeaderDraw();
        AnsiConsole.Progress()
            .Start(pContext =>
            {
                Dictionary<TTaskId, ProgressTask> tasks = new Dictionary<TTaskId, ProgressTask>();
                foreach (KeyValuePair<TTaskId, TTask> kvp in Handlers)
                {
                    ProgressTask task = pContext.AddTask(kvp.Key.ToString(), true, kvp.Value.TaskCount);
                    tasks.Add(kvp.Key, task);
                }
                
                if (_task == null)
                    return;
                
                while (!_task.IsCompleted)
                {
                    foreach (KeyValuePair<TTaskId, TTask> kvp in Handlers)
                    {
                        tasks[kvp.Key].Value(kvp.Value.Progress);
                    }
                }
                if (!_task.IsCompletedSuccessfully)
                    return;
                
                foreach (KeyValuePair<TTaskId, TTask> kvp in Handlers)
                {
                    // Force it to 100%
                    tasks[kvp.Key].Value(kvp.Value.TaskCount);
                }
            });
        
        if (_task?.Status == TaskStatus.RanToCompletion)
        {
            OnComplete();
            return;
        }
        
        OnTaskError();
    }
    
    protected virtual void OnComplete()
    {
        ConsoleUtility.WriteMarkedLine("Task Completed", EConsoleMark.Check);
        Console.WriteLine();
        StateMachine.Goto(DefaultCompleteState);
    }
    
    protected virtual void OnTaskError()
    {
        ConsoleUtility.WriteMarkedLine($"{GetType().Name} Task Error: {_task?.ToString() ?? "Task is NULL"}; Exception: {_task?.Exception?.ToString() ?? "NULL"}", EConsoleMark.Error);
        LogHandler.Error(DefaultErrorCode, $"{GetType().Name} Task Error: {_task?.ToString() ?? "Task is NULL"}; Exception: {_task?.Exception?.ToString() ?? "NULL"}");
        StateMachine.Goto(EProgramState.Log);
    }
}
