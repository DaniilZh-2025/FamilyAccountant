using FamilyAccountant.Application.Services.Authentication.Models;
using FamilyAccountant.Domain.Exceptions;
using FamilyAccountant.Domain.Repositories;
using FamilyAccountant.Domain.Services;
using Microsoft.Extensions.Logging;

namespace FamilyAccountant.Application.Services.Authentication;

public class RefreshTokenService(
    IUserRepository userRepository,
    IHasher hasher,
    ITokenService tokenService,
    ILogger<RefreshTokenService> logger)
{
    public async Task<TokenDto> Refresh(string? refreshToken)
    {
        var userLogin = tokenService.GetUserLogin(refreshToken);

        if (userLogin == null)
            throw new InvalidCredentials();

        var user = await userRepository.FindBy(userLogin);

        if (user is null ||
            user.RefreshTokenUtcExpiresAt is null ||
            user.RefreshTokenUtcExpiresAt <= DateTime.UtcNow ||
            string.IsNullOrWhiteSpace(refreshToken) ||
            string.IsNullOrWhiteSpace(user.RefreshTokenHash) ||
            !hasher.VerifyHash(refreshToken,  user.HashSalt, user.RefreshTokenHash))
            throw new InvalidCredentials();
        
        var token = tokenService.GenerateToken(user);
        
        user.RefreshTokenHash = hasher.Hash(token.RefreshToken, user.HashSalt).Hash;
        user.RefreshTokenUtcExpiresAt = DateTime.UtcNow.AddMinutes(token.RefreshExpiresInMinutes);
        
        await userRepository.UpdateRefreshToken(user);
        
        logger.LogInformation($"Refresh token for {user.Login} updated");
        
        return token;
    }
}