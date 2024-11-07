namespace ChroniclesExporter.States;

public enum EProgramState
{
    Init,
    DbLogin,
    DbTest,

    Index,
    MdRead,
    DbWrite,
    DbLink,

    Log,
    Complete
}
