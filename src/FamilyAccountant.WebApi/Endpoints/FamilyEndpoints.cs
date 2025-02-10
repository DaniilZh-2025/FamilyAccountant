using FamilyAccountant.Application.Services.Family;
using FamilyAccountant.Application.Services.Family.Models;
using Microsoft.AspNetCore.Mvc;

namespace FamilyAccountant.Endpoints;

public static class FamilyEndpoints
{
    public static void MapFamilyEndpoints(this IEndpointRouteBuilder builder)
    {
        var group = builder.MapGroup("families")
            .RequireAuthorization()
            .WithTags("Families");

        group.MapPost("", async (FamilyService s) =>
        {
            var family = await s.Create();

            return Results.Created("", family);
        })
        .WithSummary("Create family");

        group.MapGet("members", async (FamilyService s) => await s.GetMembers())
            .WithSummary("Returns all family members");

        group.MapPost("members", async (UserDto user, FamilyService s) => await s.AddMember(user))
            .WithSummary("Add family member")
            .WithDescription("Only head of family can add members");

        group.MapDelete("members/{userLogin}", async ([FromRoute] string userLogin, FamilyService s) =>
        {
            await s.RemoveMember(userLogin);
            
            return Results.NoContent();
        })
            .WithSummary("Remove family member")
            .WithDescription("Only head of family can remove members. Head of family cannot remove himself");
    } 
}