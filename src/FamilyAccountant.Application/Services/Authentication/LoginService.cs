using FamilyAccountant.Application.Services.Authentication.Models;
using FamilyAccountant.Domain.Exceptions;
using FamilyAccountant.Domain.Repositories;
using FamilyAccountant.Domain.Services;
using Microsoft.Extensions.Logging;

namespace FamilyAccountant.Application.Services.Authentication;

public class LoginService(
    IUserRepository userRepository,
    IHasher hasher,
    ITokenService tokenService,
    ILogger<LoginService> logger)
{
    public async Task<TokenDto> Login(UserCredentialDto userDto)
    {
        if (userDto.Login is null || userDto.Password is null)
            throw new InvalidCredentials();

        var user = await userRepository.FindBy(userDto.Login);

        if (user is null || !hasher.VerifyHash(userDto.Password, user.HashSalt, user.PasswordHash))
            throw new InvalidCredentials();
        
        var token = tokenService.GenerateToken(user);

        user.RefreshTokenHash = hasher.Hash(token.RefreshToken, user.HashSalt).Hash;
        user.RefreshTokenUtcExpiresAt = DateTime.UtcNow.AddMinutes(token.RefreshExpiresInMinutes);

        await userRepository.UpdateRefreshToken(user);
        
        logger.LogInformation($"User {user.Login} logged in.");
        
        return token;
    }
}