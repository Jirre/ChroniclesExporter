using ChroniclesExporter.Log;
using ChroniclesExporter.Table;
using MySqlConnector;

namespace ChroniclesExporter.IO.MySql;

/// <summary>
/// Interface used as flag in search through reflection
/// </summary>
public interface ILinkWriter;

public abstract class MySqlLinkWriter<T> : MySqlWriter<ILink>, ILinkWriter
    where T : ILink
{
    public abstract ELink LinkId { get; }
    public sealed override Enum Id => LinkId;

    protected override async Task WriteAsync(ILink[] pQueries)
    {
        try
        {
            await using MySqlConnection connection = Connection;
            await Connection.OpenAsync();
            await using MySqlCommand command = BuildCommand();
            await command.PrepareAsync();
            foreach (ILink query in pQueries)
            {
                FillCommand(command, (T)query);
                await command.ExecuteNonQueryAsync();
                ++Progress;
            }
        }
        catch (MySqlException ex)
        {
            LogHandler.Error(ELogCode.MySqlError, ex.ToString());
        }
    }
    
    protected abstract MySqlCommand BuildCommand();

    protected virtual void FillCommand(MySqlCommand pCommand, T pData)
    {
        pCommand.Parameters[0].Value = pData.Source.ToByteArray(true);
        pCommand.Parameters[1].Value = pData.Target.ToByteArray(true);
    }
}