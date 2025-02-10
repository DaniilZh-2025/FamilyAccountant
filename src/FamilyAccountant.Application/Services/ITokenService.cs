using FamilyAccountant.Application.Services.Authentication.Models;
using FamilyAccountant.Domain.Entities;

namespace FamilyAccountant.Application.Services;

public interface ITokenService
{
    TokenDto GenerateToken(User user);
    string? GetUserLogin(string? token);
}