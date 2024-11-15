namespace ChroniclesExporter.Parse;

public partial class ParseHandler
{
    private static ParseHandler? _instance;
    private static ParseHandler GetInstance() => _instance ??= new ParseHandler();

    private ParseHandler()
    {
        CreateLinkParseMethods();
    }
}
