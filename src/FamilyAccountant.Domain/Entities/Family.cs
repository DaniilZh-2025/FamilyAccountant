namespace FamilyAccountant.Domain.Entities;

public class Family
{
    public int Id { get; set; }
    public string FamilyLink { get; set; } = null! ;

    public List<User> Members { get; set; } = null!;
}