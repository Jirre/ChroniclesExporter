using ChroniclesExporter.IO.Database;

namespace ChroniclesExporter;

[DbEnum("actionTypes")]
public enum EActionTypes
{
    Action,
    Bonus,
    Reaction,
    Movement,
    Free
}
