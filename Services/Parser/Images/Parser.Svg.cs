using System.Reflection;
using System.Text.Json;
using System.Text.RegularExpressions;
using ChroniclesExporter.Log;
using ChroniclesExporter.Utility;
using HtmlAgilityPack;

namespace ChroniclesExporter.Parse;

public static partial class ParserSvg
{
    [GeneratedRegex(@"https:\/\/www\.notion\.so\/icons\/(.*?)(?:_[^\/]*)?\.svg")]
    private static partial Regex SvgRegex();
    
    private const string SVG_JSON_PATH = "ChroniclesExporter.Services.Parser.Images.SvgPaths.json";
    private static Dictionary<string, string>? _svgNamePaths;
    private static Dictionary<string, string> SvgNamePaths => _svgNamePaths ??= ReadSvgPaths();

    [ImageParseFunction(100)]
    // ReSharper disable once UnusedMember.Local
    private static bool GetSvg(string pHref, ref HtmlDocument pDoc, HtmlNode pNode)
    {
        if (!pHref.TryMatch(SvgRegex(), out Match localMatch)) return false;
        HtmlNode parent = pNode.ParentNode;
        
        HtmlNode svg = pDoc.CreateElement("SvgIcon");
        string path = localMatch.Groups[1].Value;
        if (SvgNamePaths.TryGetValue(path, out string? value))
            path = value;
        svg.SetAttributeValue("path", path);

        svg.InnerHtml = pNode.InnerHtml;
        parent.ReplaceChild(svg, pNode);
        return true;
    }

    private static Dictionary<string, string> ReadSvgPaths()
    {
        Assembly assembly = Assembly.GetExecutingAssembly();
        using Stream? stream = assembly.GetManifestResourceStream(SVG_JSON_PATH);
        if (stream == null)
        {
            LogHandler.Error(ELogCode.MdReaderError, $"Failed to load SVG JSON file: {assembly.GetName().Name}, path: {SVG_JSON_PATH}");
            _svgNamePaths = new Dictionary<string, string>();
            return _svgNamePaths;
        }

        using StreamReader reader = new StreamReader(stream);
        string json = reader.ReadToEnd();
        _svgNamePaths = JsonSerializer.Deserialize<Dictionary<string, string>>(json);
        return _svgNamePaths ??= new Dictionary<string, string>();
    }
}
