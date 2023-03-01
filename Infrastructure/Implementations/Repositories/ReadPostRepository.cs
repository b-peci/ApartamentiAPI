using System.Data;
using Application.Interfaces.Repositories;
using Application.Models;
using Dapper;
using Domain.Enums;
using Microsoft.Extensions.Configuration;
using Npgsql;
using Serilog;

namespace Infrastructure.Implementations.Repositories;

public class ReadPostRepository : IReadPostRepository
{
    
    private readonly IConfiguration _configuration;

    public ReadPostRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    
    public async Task<bool> IsUserPostCreator(Guid userId, Guid postId)
    {
        using IDbConnection connection = new NpgsqlConnection(_configuration["ConnectionStrings:Default"]);
        var isUserCreator = await connection.QueryFirstAsync<bool>(sql: "is_user_post_creator", param: new
        {
            _postid = postId,
            _userid = userId
        } ,commandType: CommandType.StoredProcedure);
        return isUserCreator;
    }

    public async Task<IEnumerable<PostThumbnailData>> GetFilteredPostsThumbnail(string country, string city, PropertyStatus status, PropertyType type, decimal minPrice,
        decimal maxPrice, int noRooms, int pageNumber)
    {
        int offsetNumber = 10 * (pageNumber - 1);
        var sqlBuilder = new SqlBuilder();
        var postSelectionTemplate = sqlBuilder.AddTemplate("SELECT p.id, " +
                                                           "p.title, " +
                                                           "SUBSTRING(p.description::text, 0, 160), " +
                                                           "p.price," +
                                                           "(SELECT images.filepath FROM images WHERE images.ismainimage and images.postid = p.id ) as MainImagePath " +
                                                           "FROM posts as p /**where**/ LIMIT 10 OFFSET @offsetNumber", new {offsetNumber});
        if ( !string.IsNullOrEmpty(country.Trim()))
        {
            sqlBuilder.Where("country = @country", new { country });
        }

        if (!string.IsNullOrEmpty(city.Trim()))
        {
            sqlBuilder.Where("city = @city", new { city });
        }

        if (status is not PropertyStatus.Default)
        {
            sqlBuilder.Where("status = @status", new { status });
        }

        if (type is not PropertyType.Default)
        {
            sqlBuilder.Where("type = @type", new { type });
        }

        if (minPrice is not 0)
        {
            sqlBuilder.Where("price >= @minPrice", new { minPrice });
        }

        if (maxPrice is not 0)
        {
            sqlBuilder.Where("price <= @maxPrice", new { maxPrice });
        }

        if (noRooms is not 0)
        {
            sqlBuilder.Where("norooms = @noRooms", new { noRooms });
        }

        try
        {
            using IDbConnection connection = new NpgsqlConnection(_configuration["ConnectionStrings:Default"]);
            var result =
                await connection.QueryAsync<PostThumbnailData>(postSelectionTemplate.RawSql,
                    postSelectionTemplate.Parameters);
            return result;
        }
        catch (Exception e)
        {
            Log.Error(e.Message);
            return null;
        }
      
    }
}