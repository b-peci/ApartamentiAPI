using System.Data;
using Application.Interfaces.Repositories;
using Domain.Entities;
using Microsoft.Extensions.Configuration;
using Dapper;
using Npgsql;

namespace Infrastructure.Implementations.Repositories;

public class UserRepository : IUserRepository
{
    private readonly IConfiguration _config;

    public UserRepository(IConfiguration config)
    {
        _config = config;
    }

    public async Task<bool> CheckCredentials(string email, string password)
    {
        using IDbConnection connection = new NpgsqlConnection(_config["ConnectionStrings:Default"]);
        var result = await connection.QueryFirstAsync<bool>(
            sql: "uf_checkcredentials",
            param: new
            {
                _email = email,
                _password = password
            },
            commandType: CommandType.StoredProcedure
        );
        return result;
    }

    public async Task<bool> IsEmailTaken(string email)
    {
        try
        {
            using IDbConnection connection = new NpgsqlConnection(_config["ConnectionStrings:Default"]);
            var result = await connection.QueryFirstAsync<bool>(
                sql: "uf_is_email_taken",
                param: new
                {
                    _email = email
                },
                commandType: CommandType.StoredProcedure
            );
            return result;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
       
    }

    public async Task ApproveUser(Guid userId)
    {
        using IDbConnection connection = new NpgsqlConnection(_config["ConnectionStrings:Default"]);
        await connection.ExecuteAsync(
            sql: "CALL up_confirm_email(@_userid)",
            param: new
            {
                _userid = userId
            }
        );
    }

    public async Task AddUser(User user)
    {
        using IDbConnection connection = new NpgsqlConnection(_config["ConnectionStrings:Default"]);
        var procedure =
            "CALL up_createuser(@_email, @_password, @_salt, @_name, @_lastname, @_dateofbirth, @_roleid)";
        var p = new DynamicParameters();
        p.Add("@_email", user.Email);
        p.Add("@_password", user.Password);
        p.Add("@_salt", user.Salt);
        p.Add("@_name", user.Name);
        p.Add("@_lastname", user.LastName);
        p.Add("@_dateofbirth", user.DateOfBirth, DbType.Date);
        p.Add("@_roleid", user.RoleId);
        await connection.ExecuteAsync(procedure, p);
    }

    public async Task<(bool isSuccess, Exception? ex)> UpdateUserBasicInfo(Guid userId, string firstName, string lastName)
    {
        try
        {
            using (IDbConnection connection = new NpgsqlConnection(_config["ConnectionStrings:Default"]))
            {
                string procedureCall = "CALL up_update_user_basic_info(@_firstname, @_lastname, @_userid)";
                var procedureParameters = new DynamicParameters();
                procedureParameters.Add("@_firstname", firstName);
                procedureParameters.Add("@_lastname", lastName);
                procedureParameters.Add("@_userid", userId);
                await connection.ExecuteAsync(procedureCall, procedureParameters);
                return (true, null);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return (false, new Exception("Could not add user"));
        }
    }

    public async Task<Guid> GetUserIdFromEmail(string email)
    {
        using IDbConnection connection = new NpgsqlConnection(_config["ConnectionStrings:Default"]);
        var result = await connection.QueryFirstAsync<Guid>(
            sql: "uf_get_user_id_from_email",
            param: new
            {
                _email = email
            },
            commandType: CommandType.StoredProcedure);
        return result;
    }

    public async Task<string> GetUserEmailFromId(Guid id)
    {
        using (IDbConnection connection = new NpgsqlConnection(_config["ConnectionStrings:Default"]))
        {
            string? result = await connection.QueryFirstAsync<string>(
                sql: "uf_get_email_by_user_id",
                param: new
                {
                    _userid = id
                },
                commandType: CommandType.StoredProcedure
            );
            return result;
        }
    }

    public async Task<byte[]> GetSaltForUser(string email)
    {
        using IDbConnection connection = new NpgsqlConnection(_config["ConnectionStrings:Default"]);
        var result = await connection.QueryFirstAsync<byte[]>(
            sql: "uf_get_salt_by_email",
            param: new
            {
                _email = email
            },
            commandType: CommandType.StoredProcedure
        );
        return result;
    }

    public async Task<string> GetUserFullNameFromUsername(string email)
    {
        using IDbConnection connection = new NpgsqlConnection(_config["ConnectionStrings:Default"]);
        var result = await connection.QueryFirstAsync<string>(
            sql: "uf_get_name_by_email",
            param: new
            {
                _email = email
            },
            commandType: CommandType.StoredProcedure
        );
        return result;
    }

    public async Task<bool> AddUserFromOAuth(User user)
    {
        try
        {
            using IDbConnection connection = new NpgsqlConnection(_config["ConnectionStrings:Default"]);
            await connection.ExecuteAsync("CALL up_add_user_from_oauth(@_email, @_password, @_firstName, @_lastName, @_roleId, @_salt)", new
            {
                _email = user.Email,
                _password = user.Password,
                _firstName = user.Name,
                _lastName = user.LastName,
                _roleId = user.RoleId,
                _salt = user.Salt
            });
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return false;
        }
    }
}