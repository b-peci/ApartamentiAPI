using System.Data;
using Application.Features.Chats.ViewModels;
using Application.Interfaces.Repositories;
using Application.Models;
using Dapper;
using Domain.Views;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace Infrastructure.Implementations.Repositories;

public class ChatRepository : IChatRepository
{
    private readonly IConfiguration _configuration;

    public ChatRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    public async Task<List<UserChat>> GetUserChats(Guid userId)
    {
        try
        {
            using IDbConnection connection = new NpgsqlConnection(_configuration["ConnectionStrings:Default"]);
            var userChats = await connection.QueryAsync<UserChat>(sql: "uf_get_user_chats", param: new
            {
                _userid = userId
            }, commandType: CommandType.StoredProcedure);
            return userChats.ToList();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<List<VChatContent>> GetChatContent(Guid firstUserId, Guid secondUserId)
    {
        try
        {
            using IDbConnection connection = new NpgsqlConnection(_configuration["ConnectionStrings:Default"]);
            var response = await connection.QueryAsync<VChatContent>(sql: "uf_get_chat_content",
                param: new
                {
                    _firstuserid = firstUserId,
                    _seconduserid = secondUserId
                }, commandType: CommandType.StoredProcedure);
            return response.ToList();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task AddMessage(SentMessageViewModel viewModel)
    {
        try
        {
            using IDbConnection connection = new NpgsqlConnection(_configuration["ConnectionStrings:Default"]);
            string sql = "CALL up_add_message(@_fromuserid, @_touserid, @_message)";
            var p = new DynamicParameters();
            p.Add("_fromuserid", viewModel.FromUserId);
            p.Add("_touserid",  viewModel.ToUserId);
            p.Add("_message", viewModel.Message);
            await connection.ExecuteAsync(sql, p);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task SetChatMessagesAsRead(Guid fromUserId, Guid toUserId)
    {
        try
        {
            using IDbConnection connection = new NpgsqlConnection(_configuration["ConnectionStrings:Default"]);
            string sqlBody = "CALL up_set_chat_messages_as_read(@_fromuserid, @_touserid)";
            await connection.ExecuteAsync(sqlBody, param: new
            {
                _touserid = toUserId,
                _fromuserid = fromUserId
            });
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}