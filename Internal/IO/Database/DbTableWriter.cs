using ChroniclesExporter.Database;
using ChroniclesExporter.Log;
using ChroniclesExporter.Settings;
using ChroniclesExporter.Table;
using Npgsql;

namespace ChroniclesExporter.IO.Database;

/// <summary>
///     Interface used as flag in search through reflection
/// </summary>
public interface ITableWriter;

public abstract class DbTableWriter<T> : DbWriter<IRow>, ITableWriter
    where T : IRow
{
    protected abstract ETable TableId { get; }

    public sealed override Enum Id => TableId;

    protected abstract string TableName { get; }
    protected abstract string[] Fields { get; }

    protected sealed override async Task WriteAsync(IRow[] pQueries)
    {
        try
        {
            await using NpgsqlConnection connection = DbHandler.DataSource.CreateConnection();
            await connection.OpenAsync();

            NpgsqlCommand truncate = connection.CreateCommand();
            truncate.CommandText = $"TRUNCATE TABLE {TableName} CASCADE";
            await truncate.ExecuteNonQueryAsync();

            await WaitForDependencies();

            string fields = "";
            foreach (string field in Fields) fields += field + ", ";
            fields = fields.Trim(' ', ',');

            string sql = $"COPY {TableName} ({fields}) FROM STDIN (FORMAT BINARY)";
            await using NpgsqlBinaryImporter importer = await connection.BeginBinaryImportAsync(
                $"COPY {TableName} ({fields}) FROM STDIN (FORMAT BINARY)");
            foreach (IRow query in pQueries)
            {
                await importer.StartRowAsync();
                await ImportRow(importer, (T) query);
                ++Progress;
            }

            await importer.CompleteAsync();
        }
        catch (NpgsqlException ex)
        {
            LogHandler.Error(ELogCode.DatabaseError, ex.ToString());
        }
    }

    private async Task WaitForDependencies()
    {
        if (!SettingsHandler.TryGetSettings(TableId, out ISettings settings) ||
            settings.Dependencies.Length == 0)
            return;

        foreach (ETable dependency in settings.Dependencies)
            if (DbHandler.TryGetWriter(dependency, out DbWriter<IRow> writer) && writer.Task != null)
                await writer.Task;
    }

    protected abstract Task ImportRow(NpgsqlBinaryImporter pImporter, T pData);
}
