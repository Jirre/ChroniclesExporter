namespace ChroniclesExporter.Log;

public enum ELogCode
{
    FileNotFound = 101,

    IndexerPathNotFound = 201,
    IndexerGuidNotFound = 202,
    IndexerGuidCollision = 205,

    MdReaderError = 300,

    MySqlError = 400
}
