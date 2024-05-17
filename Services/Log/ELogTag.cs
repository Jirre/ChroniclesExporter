namespace ChroniclesExporter.Log;

public enum ELogTag
{
    Ok = 0,
    FileNotFound = 1,
    
    StateNotFound = 10,
    
    IndexerPathNotFound = 100,
    IndexerGuidNotFound = 101,
    IndexerForcedCollision = 105,
    
    MySqlError = 200,
}
