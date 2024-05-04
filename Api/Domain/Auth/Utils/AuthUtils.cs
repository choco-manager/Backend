using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Api.Configuration;
using Api.Data.Models;
using FastEndpoints.Security;

namespace Api.Domain.Auth.Utils;

public class AuthUtils
{
    public static void CreatePasswordHash(string plainPassword, out byte[] passwordSalt, out byte[] passwordHash)
    {
        using var hmac = new HMACSHA512();
        passwordSalt = hmac.Key;
        passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(plainPassword));
    }

    public static void CreateAccessToken(SecurityConfiguration configuration, User user, out string accessToken)
    {
        accessToken = JwtBearer.CreateToken(opts =>
            {
                opts.SigningKey = configuration.SigningKey;
                opts.ExpireAt = DateTime.UtcNow.AddMinutes(40);
                opts.User.Claims.Add(new Claim(ClaimTypes.Name, user.Login));
            }
        );
    }

    public static void CreateRefreshToken(SecurityConfiguration configuration, User user, out string refreshToken,
        out byte[] salt)
    {
        using var hmac = new HMACSHA256();
        var inputData = $"{user.Id}:{hmac.Key}:{configuration.RefreshTokenSecret}";
        var inputDataBytes = Encoding.UTF8.GetBytes(inputData);

        var hashBytes = hmac.ComputeHash(inputDataBytes);
        refreshToken = Convert.ToBase64String(hashBytes);
        salt = hmac.Key;
    }
}