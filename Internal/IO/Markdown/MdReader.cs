using ChroniclesExporter.Table;
using ChroniclesExporter.Utility;
using Markdig;

namespace ChroniclesExporter.IO;

public abstract class MdReader<T> : IReader
    where T : IRow
{
    private static readonly MarkdownPipeline PIPELINE = 
        new MarkdownPipelineBuilder().UseSoftlineBreakAsHardlineBreak().Build();
    
    public Task Read(string[] pFiles)
    {
        List<Task> tasks = new List<Task>();
        foreach (string file in pFiles)
        {
            if (!File.Exists(file))
                continue;

            tasks.Add(ReadFile(file));
        }
        
        return Task.WhenAll(tasks);
    }

    private async Task ReadFile(string pFile)
    {
        string[] lines = await File.ReadAllLinesAsync(pFile);
        T row = Activator.CreateInstance<T>();
        if (!StringUtility.TryExtractGuidFromString(pFile, out Guid id))
            return;

        if (!TableHandler.TryGet(id, out TableEntry entry))
            return;
        
        row.Id = id;
        string content = "";
        foreach (string line in lines)
        {
            if (TryGetName(line, ref row) ||
                TryGetProperties(line, ref row) ||
                (string.IsNullOrWhiteSpace(line) && string.IsNullOrWhiteSpace(content)))
                continue;

            content += content + Environment.NewLine;
        }

        row.Content = Markdown.ToHtml(content, PIPELINE);
        entry.Row = row;
    }

    private static bool TryGetName(string pLine, ref T pData)
    {
        if (!pLine.StartsWith("# ") ||
            !string.IsNullOrWhiteSpace(pData.Name))
            return false;

        pData.Name = pLine.TrimStart("# ");
        return true;
    }

    protected virtual bool TryGetProperties(string pLine, ref T pData) => false;
}
