using System.Security.Claims;
using FamilyAccountant.Application.Models;
using FamilyAccountant.Application.Services;
using FamilyAccountant.Domain.Exceptions;
using FamilyAccountant.Domain.Repositories;
using FamilyAccountant.Infrastructure.Services.Models;
using Microsoft.AspNetCore.Http;

namespace FamilyAccountant.Infrastructure.Services;

public class CurrentUserService(IHttpContextAccessor httpContextAccessor, IUserRepository userRepository) : ICurrentUserService
{
    private CurrentUser? _currentUser;
    public async Task<ICurrentUser> GetCurrentUser()
    {
        if (_currentUser != null)
            return _currentUser;

        var claimsPrincipal = httpContextAccessor.HttpContext?.User;

        if (claimsPrincipal?.Identity?.IsAuthenticated is null or false)
            throw new UnauthorizedAccessException("User is not authenticated.");
        
        var userLogin = claimsPrincipal.FindFirst(ClaimTypes.Name)?.Value
            ?? throw new UnauthorizedAccessException("User ID claim is missing or invalid.");
            

        var user = await userRepository.FindBy(userLogin)
            ?? throw new NotFound("User not found");

        _currentUser = new CurrentUser
        {
            UserId = user.Id,
            UserLogin = user.Login,
            FamilyId = user.FamilyId,
            IsAdmin = user.IsAdmin,
        };

        return _currentUser;
    }
}