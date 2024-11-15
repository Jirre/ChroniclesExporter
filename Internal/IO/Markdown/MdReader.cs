using ChroniclesExporter.Log;
using ChroniclesExporter.Parse;
using ChroniclesExporter.Table;
using ChroniclesExporter.Utility;
using HtmlAgilityPack;
using Markdig;

namespace ChroniclesExporter.IO;

public abstract class MdReader<T> : IReader
    where T : class, IRow
{
    private readonly MarkdownPipeline _pipelineBuilder =
        new MarkdownPipelineBuilder().UseSoftlineBreakAsHardlineBreak().Build();
    
    public int Progress { get; private set; }
    public int TaskCount { get; private set; }

    /// <inheritdoc />
    public Task Read(string[] pFiles)
    {
        List<Task> tasks = [];
        foreach (string file in pFiles)
        {
            if (!File.Exists(file))
                LogHandler.Warning(ELogCode.FileNotFound, $"Path: {file}");
            else tasks.Add(ReadFile(file));
        }
        TaskCount = tasks.Count;
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
        for (int i = 0; i < lines.Length; i++)
        {
            string line = lines[i];
            if (i == 0 && TryGetName(line, ref row))
                continue;

            line = CleanLine(line);
            if (TryGetProperties(line, ref row) ||
                (string.IsNullOrWhiteSpace(line) && string.IsNullOrWhiteSpace(content)))
                continue;

            content += line + Environment.NewLine;
        }

        string html = Markdown.ToHtml(content, _pipelineBuilder);
        HtmlDocument htmlDoc = new();
        htmlDoc.LoadHtml(html);
        ModifyHtml(ref htmlDoc);
        row.Content = htmlDoc.DocumentNode.OuterHtml;

        entry.Row = row;
        Progress++;
    }

    private static void ModifyHtml(ref HtmlDocument pDoc)
    {
        AddLinkComponents(ref pDoc);
    }

    private static void AddLinkComponents(ref HtmlDocument pDoc)
    {
        HtmlNodeCollection anchorNodes = pDoc.DocumentNode.SelectNodes("//a");
        if (anchorNodes == null)
            return;
        
        foreach (HtmlNode node in anchorNodes)
        {
            string href = node.GetAttributeValue("href", "");
            ParseHandler.ParseLink(href, ref pDoc, node);
        }
    }

    #region --- Properties ---

    private static bool TryGetName(string pLine, ref T pData)
    {
        if (!pLine.StartsWith("# ") ||
            !string.IsNullOrWhiteSpace(pData.Name))
            return false;

        pData.Name = pLine.TrimStart("# ");
        return true;
    }

    protected virtual bool TryGetProperties(string pLine, ref T pData)
    {
        return false;
    }
    

    #endregion

    #region --- Line Cleanup ---

    private static string CleanLine(string pLine)
    {
        pLine = TrimAsterisk(pLine);
        return pLine;
    }

    private static string TrimAsterisk(string pLine)
    {
        string str;
        while (true)
        {
            str = pLine.Replace("****", "***");
            if (str.Length != pLine.Length)
                pLine = str;
            else break;
        }

        return str;
    }

    #endregion
}
