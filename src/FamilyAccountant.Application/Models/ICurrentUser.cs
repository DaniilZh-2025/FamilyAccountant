namespace FamilyAccountant.Application.Models;

public interface ICurrentUser
{
    public int UserId { get; set; }
    public string UserLogin { get; set; }
    public int? FamilyId { get; set; }
    public bool? IsAdmin { get; set; }
}