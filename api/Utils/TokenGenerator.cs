using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace api.Utils;

public class TokenGenerator
{

    public string GenerateAuthToken(Guid userId, string email, string role)
    {
        // This class is responsible for generating JWT tokens
        var tokenHandler = new JwtSecurityTokenHandler();
        // This key should be stored in a secure place and should not be hardcoded
        var key = this.GetSecretKey();

        // Claims are the data that will be stored in the token
        var claims = new List<Claim>
    {
        new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        new(JwtRegisteredClaimNames.Email, email),
        new(JwtRegisteredClaimNames.Sub, userId.ToString()),
        new(ClaimTypes.Role, role)
    };

        // The token will expire in 1 hour
        // The token will be signed using the key and the algorithm
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddHours(1),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        // Create the token
        var token = tokenHandler.CreateToken(tokenDescriptor);

        // Return the token as a string
        return tokenHandler.WriteToken(token);
    }

    public byte[] GetSecretKey()
    {
        string key = Environment.GetEnvironmentVariable("JWT_SECRET_KEY") ?? "SecretKeyShouldBeLongAndSecure";

        return Encoding.UTF8.GetBytes(key);
    }

}
