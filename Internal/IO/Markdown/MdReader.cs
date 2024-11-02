using System.Text.RegularExpressions;
using ChroniclesExporter.Log;
using ChroniclesExporter.Settings;
using ChroniclesExporter.Table;
using ChroniclesExporter.Utility;
using HtmlAgilityPack;
using Markdig;

namespace ChroniclesExporter.IO;

public abstract partial class MdReader<T> : IReader
    where T : IRow
{
    private readonly MarkdownPipeline _pipelineBuilder = 
        new MarkdownPipelineBuilder().UseSoftlineBreakAsHardlineBreak().Build();
    
    public int Progress { get; private set; }
    public int TaskCount { get; private set; }
    
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
        HtmlDocument htmlDoc = new HtmlDocument();
        htmlDoc.LoadHtml(html);
        ModifyHtml(ref htmlDoc);
        row.Content = htmlDoc.DocumentNode.OuterHtml;
        
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

    private static void ModifyHtml(ref HtmlDocument pDoc)
    {
        AddLinkComponents(ref pDoc);
    }

    [GeneratedRegex(@".*\/[A-Za-z\-]+-([a-fA-F0-9]{32})(\?|$)")]
    private static partial Regex UrlRegex();
    
    [GeneratedRegex(@"[A-Za-z]+%20([a-fA-F0-9]{32})\.md")]
    private static partial Regex FileRegex();
    private static void AddLinkComponents(ref HtmlDocument pDoc)
    {
        HtmlNodeCollection anchorNodes = pDoc.DocumentNode.SelectNodes("//a");
        if (anchorNodes == null)
            return;
        
        foreach (HtmlNode node in anchorNodes)
        {
            HtmlNode parent = node.ParentNode;
            string href = node.GetAttributeValue("href", "");
            if (TryMatch(UrlRegex(), href, out Match urlMatch))
            {
                HtmlNode link = pDoc.CreateElement("PageLink");
                if (TableHandler.TryGet(new Guid(urlMatch.Groups[1].Value), out TableEntry entry) && 
                    SettingsHandler.TryGetSettings(entry.Id, out ISettings settings))
                {
                    link.SetAttributeValue("target", 
                        string.IsNullOrWhiteSpace(settings.Url) ? 
                            urlMatch.Groups[1].Value : 
                            string.Format(settings.Url, urlMatch.Groups[1].Value));
                    link.SetAttributeValue("icon", settings.LinkIcon);
                }
                else
                {
                    link.SetAttributeValue("target", urlMatch.Groups[1].Value);
                }
                
                link.InnerHtml = node.InnerHtml;
                parent.ReplaceChild(link, node);
                continue;
            }
            if (TryMatch(FileRegex(), href, out Match localMatch))
            {
                HtmlNode link = pDoc.CreateElement("LocalLink");
                link.SetAttributeValue("target", localMatch.Groups[1].Value);

                if (TableHandler.TryGet(new Guid(localMatch.Groups[1].Value), out TableEntry entry) && 
                    SettingsHandler.TryGetSettings(entry.Id, out ISettings settings))
                {
                    link.SetAttributeValue("icon", settings.LinkIcon);
                }
                
                link.InnerHtml = node.InnerHtml;
                parent.ReplaceChild(link, node);
                continue;
            }
        }
    }

    private static bool TryMatch(Regex pRegex, string pValue, out Match pMatch)
    {
        pMatch = pRegex.Match(pValue);
        return pMatch.Success;
    }
}