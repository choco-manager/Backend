﻿using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Choco.Backend.Api.Configuration;
using Choco.Backend.Api.Data.Models;
using Choco.Backend.Api.Domain.Auth.Data;
using FastEndpoints.Security;

namespace Choco.Backend.Api.Domain.Auth.Utils;

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

    public static void CreateRestorationToken(SecurityConfiguration configuration, string login,
        out string restorationToken, out byte[] salt)
    {
        using var hmac = new HMACSHA256();
        var inputData = $"{login}:{hmac.Key}:{configuration.RestorationTokenSecret}";
        var inputDataBytes = Encoding.UTF8.GetBytes(inputData);

        var hashBytes = hmac.ComputeHash(inputDataBytes);
        restorationToken = Convert.ToBase64String(hashBytes);
        salt = hmac.Key;
    }

    public static bool IsValidPassword(User user, LoginRequest req)
    {
        return VerifyPasswordHash(req.Password, user.PasswordHash, user.PasswordSalt);
    }

    private static bool VerifyPasswordHash(string plainPassword, byte[] passwordHash, byte[] passwordSalt)
    {
        using var hmac = new HMACSHA512(passwordSalt);
        var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(plainPassword));
        return computedHash.SequenceEqual(passwordHash);
    }
}