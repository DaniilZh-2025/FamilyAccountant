using FluentMigrator;

namespace FamilyAccountant.Infrastructure.Migrations.Migrations;

[Migration(1)]
public class CreateInitialTables : Migration
{
    public override void Up()
    {
        Create.Table("Family")
            .WithColumn("Id").AsInt32().PrimaryKey().Identity()
            .WithColumn("FamilyLink").AsString(36).NotNullable();
        
        Create.Table("Usr")
            .WithColumn("Id").AsInt32().PrimaryKey().Identity()
            .WithColumn("Login").AsString(25).NotNullable().Indexed("Usr_Login")
            .WithColumn("PasswordHash").AsString(44).NotNullable()
            .WithColumn("HashSalt").AsString(24).NotNullable()
            .WithColumn("RefreshTokenHash").AsString(256).Nullable()
            .WithColumn("RefreshTokenUtcExpiresAt").AsDateTime().Nullable()
            .WithColumn("FamilyId").AsInt32().ForeignKey("Family", "Id").Nullable()
            .WithColumn("IsAdmin").AsBoolean().Nullable();

        Create.Table("ExpenseCategory")
            .WithColumn("Id").AsInt32().PrimaryKey().Identity()
            .WithColumn("Name").AsString(250).NotNullable().Indexed("ExpenseCategory_Name")
            .WithColumn("HexColor").AsString(6).NotNullable();

        Create.Table("Expense")
            .WithColumn("Id").AsInt32().PrimaryKey().Identity()
            .WithColumn("UserId").AsInt32().ForeignKey("Usr", "Id").NotNullable()
            .WithColumn("FamilyId").AsInt32().ForeignKey("Family", "Id").NotNullable()
            .WithColumn("ExpenseCategoryId").AsInt32().ForeignKey("ExpenseCategory", "Id").NotNullable()
            .WithColumn("Cost").AsDecimal(18, 2).NotNullable()
            .WithColumn("Comment").AsString(256).NotNullable()
            .WithColumn("ExpenseDateTime").AsDateTime().NotNullable();
    }

    public override void Down()
    {
        Delete.Table("example");
    }
}