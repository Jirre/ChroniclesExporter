using ChroniclesExporter.Log;
using ChroniclesExporter.MySql;
using ChroniclesExporter.Settings;
using ChroniclesExporter.Table;
using MySqlConnector;

namespace ChroniclesExporter.IO.MySql;

public interface IMySqlTableWriter
{
    public ETable TableId { get; }
}

public abstract class MySqlTableWriter<T> : MySqlWriter<IRow>, IMySqlTableWriter
    where T : IRow
{
    public abstract ETable TableId { get; }
    public override Enum Id => TableId;

    protected override async Task WriteAsync(IRow[] pQueries)
    {
        try
        {
            await using MySqlConnection connection = Connection;
            await Connection.OpenAsync();
            await using MySqlCommand command = BuildCommand();
            await command.PrepareAsync();
            await WaitForDependencies();
            foreach (IRow query in pQueries)
            {
                FillCommand(command, (T)query);
                await command.ExecuteNonQueryAsync();
            }
        }
        catch (MySqlException ex)
        {
            LogHandler.Error(ELogCode.MySqlError, ex.ToString());
        }
    }

    private async Task WaitForDependencies()
    {
        if (!SettingsHandler.TryGetSettings(TableId, out ISettings settings) ||
            settings.Dependencies.Length == 0)
            return;
        
        foreach (ETable dependency in settings.Dependencies)
        {
            if (MySqlHandler.TryGetWriter(dependency, out MySqlWriter<IRow> writer) && writer.Task != null)
                await writer.Task;
        }
    }
    
    protected abstract MySqlCommand BuildCommand();

    protected abstract void FillCommand(MySqlCommand pCommand, T pData);
}