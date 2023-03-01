namespace Application.Interfaces;

public interface IPassword
{
    (string password, byte[] salt) HashPassword(string password, byte[]? salt = null);
}