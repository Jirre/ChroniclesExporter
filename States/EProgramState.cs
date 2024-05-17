namespace ChroniclesExporter.States;

public enum EProgramState
{
    Init,
    MySqlLogin,
    MySqlTest,
    
    Index,
    MdRead,
    MySqlWrite,
    MySqlLink,
    
    Complete,
    FatalError
}
