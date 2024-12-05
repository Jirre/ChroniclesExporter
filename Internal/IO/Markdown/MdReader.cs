using System.Text.RegularExpressions;
using ChroniclesExporter.Log;
using ChroniclesExporter.Parse;
using ChroniclesExporter.Table;
using ChroniclesExporter.Utility;
using HtmlAgilityPack;
using Markdig;

namespace ChroniclesExporter.IO;

public interface IMdReader : IReader
{
    void ModifyHtml(string[] pFiles);
}

public abstract partial class MdReader<T> : IMdReader
    where T : class, IRow
{
    private readonly MarkdownPipeline _pipelineBuilder =
        new MarkdownPipelineBuilder().UseSoftlineBreakAsHardlineBreak().UsePipeTables().Build();
    
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
        if (!StringUtility.TryExtractGuidFromString(pFile, out Guid id) ||
            !TableHandler.TryGet(id, out TableEntry entry))
            return;
        
        T row = Activator.CreateInstance<T>();
        row.Id = id;
        
        string content = "";
        string[] lines = await File.ReadAllLinesAsync(pFile);
        for (int i = 0; i < lines.Length; i++)
        {
            string line = lines[i];
            if (i == 0 && TryGetName(line, row))
                continue;

            line = CleanLine(line);
            if (TryGetProperties(line, row) ||
                (string.IsNullOrWhiteSpace(line) && string.IsNullOrWhiteSpace(content)))
                continue;

            content += line + Environment.NewLine;
        }

        string html = Markdown.ToHtml(content, _pipelineBuilder);
        row.Content = html;
        entry.Row = row;
        Progress++;
    }

    public void ModifyHtml(string[] pFiles)
    {
        foreach (string file in pFiles)
        {
            if (!File.Exists(file))
            {
                LogHandler.Warning(ELogCode.FileNotFound, $"Path: {file}");
                continue;
            }
            if (!StringUtility.TryExtractGuidFromString(file, out Guid id) ||
                    !TableHandler.TryGet(id, out TableEntry entry) ||
                    entry.Row == null)
                continue;
            
            HtmlDocument htmlDoc = new();
            htmlDoc.LoadHtml(entry.Row.Content);
            AddLinkComponents(ref htmlDoc);
            ConvertImageComponents(ref htmlDoc);
            ConvertHeaderComponents(ref htmlDoc);
            entry.Row.Content = htmlDoc.DocumentNode.OuterHtml;
        }
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
    
    private static void ConvertImageComponents(ref HtmlDocument pDoc)
    {
        HtmlNodeCollection anchorNodes = pDoc.DocumentNode.SelectNodes("//img");
        if (anchorNodes == null)
            return;
        
        foreach (HtmlNode node in anchorNodes)
        {
            string src = node.GetAttributeValue("src", "");
            ParseHandler.ParseImage(src, ref pDoc, node);
        }
    }

    private static void ConvertHeaderComponents(ref HtmlDocument pDoc)
    {
        for (int i = 3; i >= 1; i--)
        {
            HtmlNodeCollection headerNodes = pDoc.DocumentNode.SelectNodes($"//h{i}");
            if (headerNodes == null)
                return;
        
            foreach (HtmlNode node in headerNodes)
            {
                HtmlNode parent = node.ParentNode;
                HtmlNode target = pDoc.CreateElement($"h{i + 1}");
                target.InnerHtml = node.InnerHtml;
                parent.ReplaceChild(target, node);
            }
        }
    }

    #region --- Properties ---

    [GeneratedRegex(@"\[.*?\]")]
    private static partial Regex NameRegex();
    
    private static bool TryGetName(string pLine, T pData)
    {
        if (!pLine.StartsWith("# ") ||
            !string.IsNullOrWhiteSpace(pData.Name))
            return false;
        
        pData.Name = NameRegex().Replace(pLine.TrimStart("# "), "").Replace("  ", " ").Trim();
        return true;
    }

    protected virtual bool TryGetProperties(string pLine, T pData)
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
