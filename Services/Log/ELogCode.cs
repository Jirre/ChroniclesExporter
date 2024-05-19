namespace ChroniclesExporter.Log;

public enum ELogCode
{
    Ok = 0,
    
    FileNotFound = 1,
    StateNotFound = 2,
    TaskNotFound = 3,
    
    IndexerPathNotFound = 100,
    IndexerGuidNotFound = 101,
    IndexerForcedCollision = 105,
    
    MdReaderError = 300,
    
    MySqlError = 400,
}
