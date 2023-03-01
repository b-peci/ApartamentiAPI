using Domain.Entities;

namespace Application.Interfaces.Repositories;

public interface IUserRepository
{
    // GET
    Task<bool> CheckCredentials(string email, string password);
    Task<bool> IsEmailTaken(string email);
    Task<byte[]> GetSaltForUser(string email);
    Task<string> GetUserFullNameFromUsername(string email);
    Task<Guid> GetUserIdFromEmail(string email);

    Task<string> GetUserEmailFromId(Guid id);
    // POST
    Task<bool> AddUserFromOAuth(User user);
    Task ApproveUser (Guid userId);
    Task AddUser(User user);
    Task<(bool isSuccess, Exception? ex)> UpdateUserBasicInfo(Guid userId, string firstName, string lastName);
    

}