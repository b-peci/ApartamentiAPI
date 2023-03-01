using System.Collections;
using System.Data;
using Application.Interfaces.Repositories;
using Application.Models;
using Dapper;
using Domain.Entities;
using Microsoft.Extensions.Configuration;
using Npgsql;
using Serilog;

namespace Infrastructure.Implementations.Repositories;

public class PostRepository : IPostRepository
{
    private readonly IConfiguration _configuration;

    public PostRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<IEnumerable<PostThumbnailData>> GetPostsThumbnail(int pageNumber)
    {
        using IDbConnection connection = new NpgsqlConnection(_configuration["ConnectionStrings:Default"]);
        var postThumbnails = await connection.QueryAsync<PostThumbnailData>(sql: "uf_get_unfiltered_posts", param: new
        {
            pagenumber = pageNumber
        },
            commandType: CommandType.StoredProcedure);
        return postThumbnails;
    }

    public async Task<int> GetPostsTotalCount()
    {
        using IDbConnection connection = new NpgsqlConnection(_configuration["ConnectionStrings:Default"]);
        var postTotalCount = await connection.QueryFirstAsync<int>(sql: "uf_get_posts_item_count", commandType: CommandType.StoredProcedure);
        return postTotalCount;
    }

    public async Task AddPost(Post post, string[] imagesPath)
    {
        using IDbConnection connection = new NpgsqlConnection(_configuration["ConnectionStrings:Default"]);
        string procedure =
            "CALL up_create_post(@_userid, @_title , @_description, @_status , " +
            "@_norooms, @_space, @_price, @_type, @_isforselling, @_isforrenting, @_images)";
        var p = new DynamicParameters();
        p.Add("@_userid", post.UserId, DbType.Guid);
        p.Add("@_title", post.Title);
        p.Add("@_description", post.Description);
        p.Add("@_status", post.Status);
        p.Add("@_norooms", post.NoOfRooms);
        p.Add("@_space", post.Space);
        p.Add("@_price", post.Price, DbType.Currency);
        p.Add("@_type",post.PropertyType);
        p.Add("@_isforselling", post.IsForSelling);
        p.Add("@_isforrenting", post.IsForRenting);
        p.Add("@_images", imagesPath);
        await connection.ExecuteAsync(procedure, p);
    }

    public async Task<List<NoUserPosts>> GetPostsCountForMonth(Guid userId)
    {
        using IDbConnection connection = new NpgsqlConnection(_configuration["ConnectionStrings:Default"]);
        IEnumerable<NoUserPosts> userPostsCountForMonth = await connection.QueryAsync<NoUserPosts>(
            sql: "uf_get_user_no_posts",
            param: new
            {
                _userid = userId
            },
            commandType: CommandType.StoredProcedure
            );
        return userPostsCountForMonth.ToList();

    }

    public async Task AddPostTimeInteracted(List<Guid> postIds, Guid userId)
    {
        using IDbConnection connection = new NpgsqlConnection(_configuration["ConnectionStrings:Default"]);
        string procedure = "CALL up_add_post_interacted(@_postid, @_userid)";
        var parameters = new DynamicParameters();
        parameters.Add("_postid", postIds);
        parameters.Add("_userid", userId);
        await connection.ExecuteAsync(procedure, parameters); 
    }

    public async Task<List<NoUserPosts>> GetTimesInteracted(List<Guid> postIds)
    {
        using IDbConnection connection = new NpgsqlConnection(_configuration["ConnectionStrings:Default"]);
        IEnumerable<NoUserPosts> result = await connection.QueryAsync<NoUserPosts>(sql: "uf_get_times_interacted",
            param: new
            {
                _postids = postIds
            }, commandType: CommandType.StoredProcedure);
        return result.ToList();
    }

    public Task<List<Guid>> GetUserPostIds(Guid userId)
    {
        throw new NotImplementedException();
    }

    public async Task<List<UserPost>> GetUserPosts(Guid userId, int pageNumber)
    {
        using IDbConnection connection = new NpgsqlConnection(_configuration["ConnectionStrings:Default"]);
        IEnumerable<UserPost> result = await connection.QueryAsync<UserPost>(sql: "uf_get_user_posts",
            param: new
            {
                _userid = userId,
                _pagenumber = pageNumber
            }, commandType: CommandType.StoredProcedure);
        return result.ToList();
    }

    public async Task<EditPost> GetPostToBeEdited(Guid postId)
    {
        using IDbConnection connection = new NpgsqlConnection(_configuration["ConnectionStrings:Default"]);
        EditPost postToBeEdited = await connection.QueryFirstAsync<EditPost>(sql: "uf_get_post_to_edit",
            param: new
            {
                _postid = postId
            }, commandType: CommandType.StoredProcedure);
        return postToBeEdited;
    }

    public async Task<bool> DeletePost(Guid postId)
    {
        try
        {
            using IDbConnection connection = new NpgsqlConnection(_configuration["ConnectionStrings:Default"]);
            string procedure = "CALL up_delete_post(@_postid)";
            await connection.ExecuteAsync("CALL up_delete_post(@_postid)", param: new
            {
                _postid = postId
            });
            return true;
        }
        catch (Exception e)
        {
            Log.Error(e.Message);
            return false;
        }
        
    }

    public async Task<bool> UpdatePost(Post post, string[] imagePaths)
    {
        try
        {
            using (IDbConnection connection = new NpgsqlConnection(_configuration["ConnectionStrings:Default"]))
            {
                string procedureBody = "CALL up_update_post_and_images(@_postid, @_title, @_description, @_status, @_norooms, @_space, @_price, @_type, @_isforselling, @_isforrenting, @_images)";
                var p = new DynamicParameters();
                p.Add("_postid", post.Id);
                p.Add("_title", post.Title);
                p.Add("_description", post.Description);
                p.Add("_status", post.Status);
                p.Add("_norooms", post.NoOfRooms);
                p.Add("_space", post.Space);
                p.Add("_price", post.Price, DbType.Currency);
                p.Add("_isforselling", post.IsForSelling);
                p.Add("_isforrenting", post.IsForRenting);
                p.Add("_type", post.PropertyType);
                p.Add("_images", imagePaths);
                await connection.ExecuteAsync(sql: procedureBody, param: p);
                return true;
            }
        }
        catch (Exception e)
        {
            Log.Error(e.Message);
            return false;
        }
    }
}