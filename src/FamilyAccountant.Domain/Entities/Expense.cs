namespace FamilyAccountant.Domain.Entities;

public class Expense
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int FamilyId { get; set; }
    public int ExpenseCategoryId { get; set; }
    public decimal Cost { get; set; }
    public string? Comment { get; set; }
    public DateTime ExpenseDateTime { get; set; }

    public User User { get; set; } = null!;
    public Family Family { get; set; } = null!;
    public ExpenseCategory ExpenseCategory { get; set; } = null!;
}