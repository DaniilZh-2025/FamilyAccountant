using FamilyAccountant.Application.Services.Authentication.Models;
using FamilyAccountant.Domain.Entities;
using FamilyAccountant.Domain.Exceptions;
using FamilyAccountant.Domain.Repositories;
using FamilyAccountant.Domain.Services;
using Microsoft.Extensions.Logging;

namespace FamilyAccountant.Application.Services.Authentication;

public class RegisterService(
    IUserRepository userRepository,
    IHasher hasher,
    ILogger<RegisterService> logger)
{
    public async Task Register(UserCredentialDto userDto)
    {
        if (await userRepository.UserExist(userDto.Login))
            throw new BusinessException("User with same login already exists.");

        var password = hasher.Hash(userDto.Password);

        await userRepository.Create(new User
            { Login = userDto.Login, PasswordHash = password.Hash, HashSalt = password.Salt });
        
        logger.LogInformation($"User {userDto.Login} has been created.");
    }
}