namespace ChroniclesExporter.IO.Database;

[AttributeUsage(AttributeTargets.Enum)]
public class DbEnumAttribute(string pName) : Attribute
{
    public string DbEnumName { get; private set; } = pName;
}
