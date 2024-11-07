using ChroniclesExporter.Database;
using ChroniclesExporter.Log;
using ChroniclesExporter.Settings;
using ChroniclesExporter.Table;
using Npgsql;

namespace ChroniclesExporter.IO.Database;

/// <summary>
/// Interface used as flag in search through reflection
/// </summary>
public interface ITableWriter;

public abstract class DbTableWriter<T> : DbWriter<IRow>, ITableWriter
    where T : IRow
{
    public abstract ETable TableId { get; }
    
    public sealed override Enum Id => TableId;
    protected sealed override async Task WriteAsync(IRow[] pQueries)
    {
        try
        {
            await using NpgsqlConnection connection = DbHandler.DataSource.CreateConnection();
            await connection.OpenAsync();
            await using NpgsqlCommand command = BuildCommand();
            await command.PrepareAsync();
            await WaitForDependencies();
            foreach (IRow query in pQueries)
            {
                FillCommand(command, (T)query);
                await command.ExecuteNonQueryAsync();
                ++Progress;
            }
        }
        catch (NpgsqlException ex)
        {
            LogHandler.Error(ELogCode.MySqlError, ex.ToString());
        }
    }
    
    private async Task WaitForDependencies()
    {
        if (!SettingsHandler.TryGetSettings<T>(TableId, out ISettings<T> settings) ||
            settings.Dependencies.Length == 0)
            return;
        
        foreach (ETable dependency in settings.Dependencies)
        {
            if (DbHandler.TryGetWriter(dependency, out DbWriter<IRow> writer) && writer.Task != null)
                await writer.Task;
        }
    }
    
    protected abstract NpgsqlCommand BuildCommand();

    protected abstract void FillCommand(NpgsqlCommand pCommand, T pData);
}
