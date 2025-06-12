using System.Data.Common;
using NextUni.Common.Application.Data;
using Npgsql;

namespace NextUni.Common.Infrastructure.Data;

public class DbConnectionFactory(NpgsqlDataSource dataSource) : IDbConnectionFactory
{
    public async ValueTask<DbConnection> OpenConnectionAsync()
    {
        return await dataSource.OpenConnectionAsync();
    }
}