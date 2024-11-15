using System.Reflection;
using ChroniclesExporter.Log;
using ChroniclesExporter.Utility;
using HtmlAgilityPack;

namespace ChroniclesExporter.Parse;

public partial class ParseHandler // Links
{
    private delegate bool LinkParseMethod(string pHref, ref HtmlDocument pDoc, HtmlNode pNode);
    private IOrderedEnumerable<LinkParseMethod> _linkParseMethods = null!;
    
    private void CreateLinkParseMethods()
    {
        IEnumerable<MethodInfo> linkParsers = ReflectionUtility.GetMethodsWithAttribute(typeof(LinkParseFunctionAttribute), true);
        _linkParseMethods = linkParsers
            .Select(pMethod =>
            {
                // If the method does not match the expected signature, return null
                if (pMethod.GetParameters().Length != 3 ||
                    pMethod.GetParameters()[0].ParameterType != typeof(string) ||
                    pMethod.GetParameters()[1].ParameterType != typeof(HtmlDocument).MakeByRefType() ||
                    pMethod.GetParameters()[2].ParameterType != typeof(HtmlNode)) return null;
                
                // Create the delegate for LinkParseMethod<T>
                Type delegateType = typeof(LinkParseMethod);
                return (LinkParseMethod)Delegate.CreateDelegate(delegateType, pMethod);
            }).Where(pDelegate => pDelegate != null).OrderByDescending(pDelegate =>
            {
                MethodInfo method = pDelegate!.Method;
                LinkParseFunctionAttribute? attribute = method.GetCustomAttribute<LinkParseFunctionAttribute>();
                return attribute?.GetType().GetProperty("Priority")?.GetValue(attribute) as int? ?? 0;
            })!;
    }
    
    public static void ParseLink(string pHref, ref HtmlDocument pDoc, HtmlNode pNode)
    {
        foreach (LinkParseMethod parser in GetInstance()._linkParseMethods)
        {
            try
            {
                if (parser(pHref, ref pDoc, pNode))
                    return;
            }
            catch
            {
                LogHandler.Error(ELogCode.MdReaderError, $"Failed to parse link: {pHref}");
            }
        }
    }
}

[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
public class LinkParseFunctionAttribute(int pPriority) : Attribute
{
    public int Priority { get; init; } = pPriority;
}