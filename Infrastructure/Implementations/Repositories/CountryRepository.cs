using System.Data;
using Application.Interfaces.Repositories;
using Dapper;
using Microsoft.Extensions.Configuration;
using Npgsql;
using Serilog;

namespace Infrastructure.Implementations.Repositories;

public class CountryRepository : ICountryRepository
{
    private readonly IConfiguration _configuration;

    public CountryRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    
    public async Task<IEnumerable<string>> GetCountries()
    {
        try
        {
            using IDbConnection connection = new NpgsqlConnection(_configuration["ConnectionStrings:Default"]);
            List<string> countries = (await connection.QueryAsync<string>(sql: "uf_get_countries", commandType: CommandType.StoredProcedure)).ToList();
            return countries;
        }
        catch (Exception e)
        {
            Log.Error(e.Message);
            throw;
        }
    }
}