using FamilyAccountant.Application.Models;

namespace FamilyAccountant.Infrastructure.Services.Models;

public class CurrentUser : ICurrentUser
{
    public int UserId { get; set; }
    public string UserLogin { get; set; } = null!;
    public int? FamilyId { get; set; }
    public bool? IsAdmin { get; set; }
}