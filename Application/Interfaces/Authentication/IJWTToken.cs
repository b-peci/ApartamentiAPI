namespace Application.Interfaces;

public interface IJWTToken
{
    Task<string> CreateToken(string userName, string displayName, string userId);
}