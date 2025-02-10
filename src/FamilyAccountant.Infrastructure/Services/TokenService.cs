using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using FamilyAccountant.Application.Services;
using FamilyAccountant.Application.Services.Authentication.Models;
using FamilyAccountant.Domain.Entities;
using FamilyAccountant.Infrastructure.Constants;
using FamilyAccountant.Infrastructure.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace FamilyAccountant.Infrastructure.Services;

public class TokenService(IOptions<JwtSettings> options) : ITokenService
{
    private readonly JwtSettings _tokenSettings = options.Value;

    public TokenDto GenerateToken(User user)
    {
        var accessToken = GenerateToken(
            user.Login,
            _tokenSettings.AccessTokenExpirationInMinutes,
            [ new Claim(JwtClaims.TokenType, JwtClaimValues.Access)]);
        
        var refreshToken = GenerateToken(
            user.Login,
            _tokenSettings.RefreshTokenExpirationInMinutes,
            [ new Claim(JwtClaims.TokenType, JwtClaimValues.Refresh)]);
        
        return new TokenDto(
            accessToken,
            refreshToken,
            JwtBearerDefaults.AuthenticationScheme,
            _tokenSettings.AccessTokenExpirationInMinutes,
            _tokenSettings.RefreshTokenExpirationInMinutes);
    }

    public string? GetUserLogin(string? token)
    {
        //TODO Вынести конфигурацию
        var parameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenSettings.SecurityKey)),
        };

        var handler = new JwtSecurityTokenHandler();
        try
        {
            var principal = handler.ValidateToken(token, parameters, out _);
            var tokenType = principal.Claims.FirstOrDefault(p => p.Type == JwtClaims.TokenType)?.Value;
            if (tokenType != JwtClaimValues.Refresh)
                return null;

            var userName = principal.FindFirst(ClaimTypes.Name)?.Value;

            return userName;
        }
        catch
        {
            // ignored
        }

        return null;
    }

    private string GenerateToken(string userName, int expiresInMinutes, List<Claim> claims)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenSettings.SecurityKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        claims.Add(new Claim(JwtRegisteredClaimNames.UniqueName, userName));

        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(expiresInMinutes),
            signingCredentials: credentials);
        
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}