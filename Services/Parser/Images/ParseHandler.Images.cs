using System.Reflection;
using ChroniclesExporter.Log;
using ChroniclesExporter.Utility;
using HtmlAgilityPack;

namespace ChroniclesExporter.Parse;

public partial class ParseHandler // Images
{
    private delegate bool ImageParseMethod(string pSrc, ref HtmlDocument pDoc, HtmlNode pNode);
    private IOrderedEnumerable<ImageParseMethod> _imageParseMethods = null!;

    private void CreateImageParseMethods()
    {
        IEnumerable<MethodInfo> imageParsers = ReflectionUtility.GetMethodsWithAttribute(typeof(ImageParseFunctionAttribute), true);
        _imageParseMethods = imageParsers
            .Select(pMethod =>
            {
                // If the method does not match the expected signature, return null
                if (pMethod.GetParameters().Length != 3 ||
                    pMethod.GetParameters()[0].ParameterType != typeof(string) ||
                    pMethod.GetParameters()[1].ParameterType != typeof(HtmlDocument).MakeByRefType() ||
                    pMethod.GetParameters()[2].ParameterType != typeof(HtmlNode)) return null;
                
                // Create the delegate for LinkParseMethod
                Type delegateType = typeof(ImageParseMethod);
                return (ImageParseMethod)Delegate.CreateDelegate(delegateType, pMethod);
            }).Where(pDelegate => pDelegate != null).OrderByDescending(pDelegate =>
            {
                MethodInfo method = pDelegate!.Method;
                ImageParseFunctionAttribute? attribute = method.GetCustomAttribute<ImageParseFunctionAttribute>();
                return attribute?.GetType().GetProperty("Priority")?.GetValue(attribute) as int? ?? 0;
            })!;
    }

    public static void ParseImage(string pSrc, ref HtmlDocument pDoc, HtmlNode pNode)
    {
        foreach (ImageParseMethod parser in GetInstance()._imageParseMethods)
        {
            try
            {
                if (parser(pSrc, ref pDoc, pNode))
                    return;
            }
            catch (Exception e)
            {
                LogHandler.Error(ELogCode.MdReaderError, e.ToString());
            }
        }
    }
}

[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
public class ImageParseFunctionAttribute(int pPriority) : Attribute
{
    public int Priority { get; init; } = pPriority;
}