using FamilyAccountant.Application.Models;

namespace FamilyAccountant.Application.Services;

public interface ICurrentUserService
{
    Task<ICurrentUser> GetCurrentUser();
}