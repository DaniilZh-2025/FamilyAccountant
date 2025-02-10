// ReSharper disable NotAccessedPositionalProperty.Global
namespace FamilyAccountant.Application.Services.Authentication.Models;

public record TokenDto(
    string AccessToken,
    string RefreshToken,
    string TokenType,
    int TokenExpiresInMinutes,
    int RefreshExpiresInMinutes);