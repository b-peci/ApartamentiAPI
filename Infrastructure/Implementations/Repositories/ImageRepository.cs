using System.Data;
using Application.Interfaces.Repositories;
using Dapper;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace Infrastructure.Implementations.Repositories;

public class ImageRepository : IImageRepository
{
    
    private readonly IConfiguration _configuration;

    public ImageRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    
    public async Task<List<string>> GetPostImages(Guid postId)
    {
        using IDbConnection connection = new NpgsqlConnection(_configuration["ConnectionStrings:Default"]);
        List<string> imagePaths = (await connection.QueryAsync<string>(sql: "uf_get_post_images", param:
            new
            {
                _postid = postId
            }, commandType: CommandType.StoredProcedure)).ToList();
        return imagePaths;
    }
}