namespace FamilyAccountant.Application.Services.Authentication.Models;

public record UserCredentialDto(
    string Login,
    string Password);