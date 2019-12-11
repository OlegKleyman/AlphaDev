using Microsoft.EntityFrameworkCore.Migrations;

namespace AlphaDev.Core.Data.Sql.Migrations.Information
{
    public partial class AddContact : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                "Contacts",
                table => new
                {
                    Value = table.Column<string>(),
                    Id = table.Column<bool>()
                },
                constraints: table => { table.PrimaryKey("PK_Contacts", x => x.Id); });

            migrationBuilder.Sql(
                "ALTER TABLE Contacts ADD CONSTRAINT CK_CONTACTS_SIZE CHECK (dbo.CheckTableSize('Contacts') = 1)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                "Contacts");
        }
    }
}