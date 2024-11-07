using ChroniclesExporter.Database;
using ChroniclesExporter.Log;
using ChroniclesExporter.Table;
using Npgsql;

namespace ChroniclesExporter.IO.Database;

/// <summary>
///     Interface used as flag in search through reflection
/// </summary>
public interface ILinkWriter;

public abstract class DbLinkWriter<T> : DbWriter<ILink>, ILinkWriter
    where T : ILink
{
    public abstract ELink LinkId { get; }
    public sealed override Enum Id => LinkId;

    protected abstract string TableName { get; }
    protected abstract string[] Fields { get; }

    protected override async Task WriteAsync(ILink[] pQueries)
    {
        try
        {
            await using NpgsqlConnection connection = DbHandler.DataSource.CreateConnection();
            await connection.OpenAsync();

            NpgsqlCommand truncate = connection.CreateCommand();
            truncate.CommandText = $"TRUNCATE TABLE {TableName} CASCADE";
            await truncate.ExecuteNonQueryAsync();

            string fields = "";
            foreach (string field in Fields) fields += field + ", ";
            fields = fields.Trim(' ', ',');

            await using NpgsqlBinaryImporter importer = await connection.BeginBinaryImportAsync(
                $"COPY {TableName} ({fields}) FROM STDIN (FORMAT BINARY)");
            foreach (ILink query in pQueries)
            {
                await importer.StartRowAsync();
                await ImportRow(importer, (T) query);
                ++Progress;
            }

            await importer.CompleteAsync();
        }
        catch (NpgsqlException ex)
        {
            LogHandler.Error(ELogCode.MySqlError, ex.ToString());
        }
    }

    protected virtual async Task ImportRow(NpgsqlBinaryImporter pImporter, T pData)
    {
        await pImporter.WriteAsync(pData.Source.ToByteArray(true));
        await pImporter.WriteAsync(pData.Target.ToByteArray(true));
    }
}
