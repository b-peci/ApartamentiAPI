using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Application.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace Infrastructure.Implementations;

public class JWTToken : IJWTToken
{
    private readonly IConfiguration _conf;

    public JWTToken(IConfiguration configuration)
    {
        _conf = configuration;
    }

   

    public Task<string> CreateToken(string userName, string displayName, string userId)
    {
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub,_conf["JWT:Subject"]),
            new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Iat,DateTime.Now.ToString()),
            new Claim("username",userName),
            new Claim("displayName",displayName),
            new Claim("UserId", userId)
        };
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_conf["JWT:Key"]));
        var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(_conf["JWT:Issuer"], _conf["JWT:Audience"], claims,
            expires: DateTime.Now.AddHours(1), signingCredentials: signIn);
        return Task.FromResult(new JwtSecurityTokenHandler().WriteToken(token));
    }
}