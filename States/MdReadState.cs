using ChroniclesExporter.IO;
using ChroniclesExporter.Log;
using ChroniclesExporter.Settings;
using ChroniclesExporter.StateMachine;
using ChroniclesExporter.Table;
using Spectre.Console;

namespace ChroniclesExporter.States;

public class MdReadState(StateMachine<EProgramState> pStateMachine, EProgramState pId) :
    AProgressState<ETable, IReader>(pStateMachine, pId)
{
    protected override EProgramState DefaultCompleteState => EProgramState.DbWrite;
    protected override ELogCode DefaultErrorCode => ELogCode.MdReaderError;

    private Dictionary<ETable, List<string>>? _files;

    protected override List<Task> BuildHandlers()
    {
        _files = new Dictionary<ETable, List<string>>();
        foreach (TableEntry table in TableHandler.Entries)
        {
            if (!_files.ContainsKey(table.Id))
                _files.Add(table.Id, new List<string>());

            _files[table.Id].Add(table.Path);
        }

        List<Task> tasks = new();
        foreach (KeyValuePair<ETable, List<string>> kvp in _files)
        {
            if (!SettingsHandler.TryGetSettings(kvp.Key, out ISettings settings))
                continue;

            if (Activator.CreateInstance(settings.Reader) is not IReader reader) continue;
            Handlers.Add(kvp.Key, reader);
            tasks.Add(reader.Read(kvp.Value.ToArray()));
        }

        return tasks;
    }

    protected override void OnComplete()
    {
        foreach (KeyValuePair<ETable, IReader> kvp in Handlers)
        {
            if (kvp.Value is not IMdReader mdReader ||
                _files == null || 
                !_files.TryGetValue(kvp.Key, out List<string>? fileList))
            {
                LogHandler.Warning(ELogCode.HtmlParserError, $"{kvp.Key}: {kvp.Value}, {_files!.ContainsKey(kvp.Key)}");
                continue;
            }
            
            mdReader.ModifyHtml(fileList.ToArray());
        }

        base.OnComplete();
    }

    protected override void OnHeaderDraw()
    {
        Rule header = new("[blue]Reading Markdown Files[/]")
        {
            Justification = Justify.Left
        };
        AnsiConsole.Write(header);
    }
}
