using System.Data;
using Application.Interfaces.Repositories;
using Dapper;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace Infrastructure.Implementations.Repositories;

public class RoleRepository: IRoleRepository
{
    
    private readonly IConfiguration _configuration;

    public RoleRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    
    public async Task<Guid> GetDefaultRole()
    {
        using IDbConnection connection = new NpgsqlConnection(_configuration["ConnectionStrings:Default"]);
        Guid defaultRoleId = await connection.QueryFirstAsync<Guid>(sql: "uf_get_default_role", commandType: CommandType.StoredProcedure);
        return defaultRoleId;
    }
}