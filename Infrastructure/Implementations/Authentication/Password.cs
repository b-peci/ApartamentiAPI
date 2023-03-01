using System.Security.Cryptography;
using Application.Interfaces;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace Infrastructure.Implementations;

public class Password : IPassword
{
    public (string password, byte[] salt) HashPassword(string password, byte[]? salt = null)
    {
        //if salt is null that means we are creating a new account
        //if we pass the salt that means that we are checking if
        //the credentials meet so get the salt from db
        salt ??= new byte[128 / 8];
        using (var rngCsp = new RNGCryptoServiceProvider())
        {
            if(salt[0] == 0) rngCsp.GetNonZeroBytes(salt);
        }

        // derive a 256-bit subkey (use HMACSHA256 with 100,000 iterations)
        string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password: password,
            salt: salt,
            prf: KeyDerivationPrf.HMACSHA256,
            iterationCount: 100000,
            numBytesRequested: 256 / 8));
        
        return (hashed, salt);
    }
}