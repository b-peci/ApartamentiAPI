using System.Data;
using Application.Interfaces.Repositories;
using Dapper;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace Infrastructure.Implementations.Repositories;

public class CityRepository : ICityRepository
{
    private readonly IConfiguration _configuration;

    public CityRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    
    public async Task<IEnumerable<string>> GetCitiesByCountry(string country)
    {
        using IDbConnection connection = new NpgsqlConnection(_configuration["ConnectionStrings:Default"]);
        List<string> cities = (await connection.QueryAsync<string>(sql: "uf_get_cities_by_country",
            param: new
            {
                _country = country
            }, commandType: CommandType.StoredProcedure)).ToList();
        return cities;
    }
}