using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApplication1.Models;

namespace WebApplication1.Repository;

public class JWTManagerRepository : IJWTManagerRepository
{
    Dictionary<string, string> UserRecords = new Dictionary<string, string>
    {
        { "user1","password1"},
        { "user2","password2"},
        { "user3","password3"},
    };

    private readonly IConfiguration _configuration;
    public JWTManagerRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public Tokens Authenticate(Users users)
    {
        if (!UserRecords.Any(x => x.Key == users.Name && x.Value == users.Password))
        {
            return new Tokens();
        }

        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenKey = Encoding.UTF8.GetBytes(_configuration["JWT:Key"]);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, users.Name),
                new Claim(ClaimTypes.Role,"admin")
            }),
            Expires = DateTime.UtcNow.AddMinutes(10),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return new Tokens { Token = tokenHandler.WriteToken(token) };
    }
}
