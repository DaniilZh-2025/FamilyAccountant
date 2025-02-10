namespace FamilyAccountant.Domain.Entities;

public class ExpenseCategory
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string HexColor { get; set; } = null!;
}