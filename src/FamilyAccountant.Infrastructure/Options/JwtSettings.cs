namespace FamilyAccountant.Infrastructure.Options;

public class JwtSettings
{
    public string SecurityKey { get; set; } = null!;
    public int AccessTokenExpirationInMinutes { get; set; }
    public int RefreshTokenExpirationInMinutes { get; set; }
}