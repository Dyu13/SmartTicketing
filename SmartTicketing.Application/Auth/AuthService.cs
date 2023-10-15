using System.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;

using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

using SmartTicketing.Application.Interfaces;
using System.Security.Claims;
using System.Text.Json;
using SmartTicketing.Domain.Enums;

namespace SmartTicketing.Application.Auth;

public class AuthService : IAuthService
{
    private readonly IApplicationDbContext _dbContext;

    public AuthService(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    // TODO: make use of an identity provider
    public async Task<string> AuthenticateAsync(string username, string password)
    {
        if (string.IsNullOrWhiteSpace(username))
        {
            throw new ArgumentNullException(nameof(username));
        }

        if (string.IsNullOrWhiteSpace(password))
        {
            throw new ArgumentNullException(nameof(password));
        }

        var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Username == username);

        if (user == null)
        {
            throw new NullReferenceException(nameof(user));
        }

        if (!ComputeSha(password).Equals(user.Password))
        {
            throw new Exception("Password do not match");
        }
        
        var jwt = GenerateJwt(username, user.UserId, user.Role);
        return jwt;
    }

    private string GenerateJwt(string username, int userId, EUserRole userRole)
    {
        var lifetime = TimeSpan.FromHours(4); // TODO: add refresh token
        var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("SecretKeyForJwt-71b4c0bc-5c8c-4221-9529-c7db32c4daa0")); // TODO: should be taken from key vault
        var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(JwtRegisteredClaimNames.Sub, username),
            new(JwtRegisteredClaimNames.Email, username),
            new("userid", userId.ToString()),
            new("isAdmin", (userRole == EUserRole.Admin).ToString())
        };

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.Add(lifetime),
            Issuer = "https://host.com",
            Audience = "https://tickets.host.com",
            SigningCredentials = signingCredentials
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        var jwt = tokenHandler.WriteToken(token);

        return jwt;
    }

    private string ComputeSha(string password)
    {
        using (SHA512 sha = SHA512.Create())
        {
            byte[] hashValue = sha.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToHexString(hashValue);
        }
    }
}
