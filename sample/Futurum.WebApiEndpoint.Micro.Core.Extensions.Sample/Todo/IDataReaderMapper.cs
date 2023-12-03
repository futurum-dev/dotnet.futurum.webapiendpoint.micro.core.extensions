using Microsoft.Data.Sqlite;

namespace Futurum.WebApiEndpoint.Micro.Core.Extensions.Sample.Todo;

public interface IDataReaderMapper<T>
{
    static abstract T Map(SqliteDataReader dataReader);
}
