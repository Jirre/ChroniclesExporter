using System.Text.RegularExpressions;
using ChroniclesExporter.Log;
using ChroniclesExporter.Settings;
using ChroniclesExporter.Table;
using ChroniclesExporter.Utility;
using Markdig;

namespace ChroniclesExporter.IO;

public abstract class MdReader<T> : IReader
    where T : IRow
{
    private static readonly MarkdownPipeline PIPELINE = 
        new MarkdownPipelineBuilder().UseSoftlineBreakAsHardlineBreak().Build();
    
    public int Progress { get; private set; }
    
    /// <inheritdoc/>
    public Task Read(string[] pFiles)
    {
        List<Task> tasks = new List<Task>();
        foreach (string file in pFiles)
        {
            if (!File.Exists(file))
                LogHandler.Warning(ELogCode.FileNotFound, $"Path: {file}");
            else tasks.Add(ReadFile(file));
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
        for (int i = 0; i < lines.Length; i++)
        {
            string line = lines[i];
            if (i == 0 && TryGetName(line, ref row))
                continue;

            line = CleanLine(line);
            if (TryGetProperties(line, ref row) ||
                (string.IsNullOrWhiteSpace(line) && string.IsNullOrWhiteSpace(content)))
                continue;

            if (SanitizeMarkdown(line, out string output))
                content += output + Environment.NewLine;
        }

        row.Content = Markdown.ToHtml(content, PIPELINE);
        entry.Row = row;
        Progress++;
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

    protected virtual bool TryGetProperties(string pLine, ref T pData) => false;

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

    #region --- Markdown Cleanup ---

    private static bool SanitizeMarkdown(string pLine, out string pOutput)
    {
        pOutput = MarkdownLinkToHtml(pLine);
        return true;
    }

    private static string MarkdownLinkToHtml(string pMarkdown)
    {
        Regex regex = StringUtility.MarkdownLinkRegex();
        
        return regex.Replace(pMarkdown, pMatch =>
        {
            string text = pMatch.Groups["text"].Value;
            string url = pMatch.Groups["url"].Value;
            string title = pMatch.Groups["title"].Captures.Count > 0 ? pMatch.Groups["title"].Value : text;

            if (StringUtility.TryExtractGuidFromString(url, out Guid guid) &&
                TableHandler.TryGet(guid, out TableEntry table) &&
                SettingsHandler.TryGetSettings(table.Id, out ISettings settings))
            {
                string classes = string.IsNullOrWhiteSpace(settings.LinkClasses) ? "" : $" class=\"{settings.LinkClasses}\"";
                return $"<a href=\"{settings.Url}\"{classes}>{title}</a>";
            }
            return $"<a href=\"{url}\">{title}</a>";
        });
    }

    #endregion
}
