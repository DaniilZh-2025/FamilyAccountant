// ReSharper disable UnusedAutoPropertyAccessor.Global
namespace FamilyAccountant.Domain.Entities;

public class User
{
    public int Id { get; set; }
    public string Login { get; set; } = null!;
    public string PasswordHash { get; set; } = null!;
    public string HashSalt { get; set; } = null!;
    public string? RefreshTokenHash { get; set; }
    public DateTime? RefreshTokenUtcExpiresAt { get; set; }

    public int? FamilyId { get; set; }
    public bool? IsAdmin { get; set; }

    public Family? Family { get; set; }
}