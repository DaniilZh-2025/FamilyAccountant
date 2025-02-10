namespace FamilyAccountant.Application.Services.Family.Models;

public record FamilyMemberDto(string UserLogin, bool IsAdmin)
{
    //Only for automapper
    public FamilyMemberDto() : this(null!, false) { }
}