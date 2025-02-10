using FamilyAccountant.Application.Services.Authentication;
using FamilyAccountant.Application.Services.Authentication.Models;
using Microsoft.AspNetCore.Mvc;

namespace FamilyAccountant.Endpoints;

public static class AccountEndpoints
{
    public static void MapAccountEndpoints(this IEndpointRouteBuilder builder)
    {
        var group = builder.MapGroup("accounts").WithTags("Accounts");

        group.MapPost("signup", async (UserCredentialDto user, RegisterService s) =>
        {
            await s.Register(user);
            
            return Results.Created();
        }).WithSummary("Creates a new account");

        group.MapPost("login", async (UserCredentialDto user, LoginService s) => await s.Login(user))
            .WithSummary("Login user");

        group.MapPost("refresh", async ([FromForm] string refreshToken, RefreshTokenService s) => await s.Refresh(refreshToken))
            .DisableAntiforgery()
            .WithSummary("Refresh access token");
        
    }
}