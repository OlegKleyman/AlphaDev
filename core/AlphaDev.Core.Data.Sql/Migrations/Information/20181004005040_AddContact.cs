using Microsoft.EntityFrameworkCore.Migrations;

namespace AlphaDev.Core.Data.Sql.Migrations.Information
{
    public partial class AddContact : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Contacts",
                columns: table => new
                {
                    Value = table.Column<string>(nullable: false),
                    Id = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contacts", x => x.Id);
                });

            migrationBuilder.Sql(
                "ALTER TABLE Contacts ADD CONSTRAINT CK_CONTACTS_SIZE CHECK (dbo.CheckTableSize('Contacts') = 1)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Contacts");
        }
    }
}
