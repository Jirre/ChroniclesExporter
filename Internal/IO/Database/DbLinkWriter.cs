using ChroniclesExporter.Database;
using ChroniclesExporter.Log;
using ChroniclesExporter.Table;
using Npgsql;

namespace ChroniclesExporter.IO.Database;

/// <summary>
/// Interface used as flag in search through reflection
/// </summary>
public interface ILinkWriter;

public abstract class DbLinkWriter<T> : DbWriter<ILink>, ILinkWriter
    where T : ILink
{
    public abstract ELink LinkId { get; }
    public sealed override Enum Id => LinkId;
    
    protected override async Task WriteAsync(ILink[] pQueries)
    {
        try
        {
            await using NpgsqlConnection connection = DbHandler.DataSource.CreateConnection();
            await connection.OpenAsync();
            await using NpgsqlCommand command = BuildCommand();
            await command.PrepareAsync();
            foreach (ILink query in pQueries)
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
    
    protected abstract NpgsqlCommand BuildCommand();

    protected virtual void FillCommand(NpgsqlCommand pCommand, T pData)
    {
        pCommand.Parameters[0].Value = pData.Source.ToByteArray(true);
        pCommand.Parameters[1].Value = pData.Target.ToByteArray(true);
    }
}
