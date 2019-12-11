using Microsoft.EntityFrameworkCore.Migrations;

namespace AlphaDev.Core.Data.Sql.Migrations.Information
{
    public partial class Inital : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                "Abouts",
                table => new
                {
                    Id = table.Column<bool>(),
                    Value = table.Column<string>()
                },
                constraints: table => { table.PrimaryKey("PK_Abouts", x => x.Id); });

            migrationBuilder.Sql(@"CREATE FUNCTION CheckTableSize(@TABLE_NAME NVARCHAR(128))
                                   RETURNS INT
                                   AS
                                   BEGIN
	                                   DECLARE @size INT
	                                   SELECT @size = SUM(rows) FROM sys.partitions
	                                   WHERE object_id = OBJECT_ID(@TABLE_NAME)
	                                   RETURN @size
                                   END");
            migrationBuilder.Sql(
                "ALTER TABLE Abouts ADD CONSTRAINT CK_ABOUTS_SIZE CHECK (dbo.CheckTableSize('Abouts') = 1)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                "Abouts");
            migrationBuilder.Sql("DROP FUNCTION dbo.CheckTableSize");
        }
    }
}