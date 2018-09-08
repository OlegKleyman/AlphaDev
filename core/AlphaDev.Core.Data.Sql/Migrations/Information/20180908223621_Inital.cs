using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace AlphaDev.Core.Data.Sql.Migrations.Information
{
    public partial class Inital : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Abouts",
                columns: table => new
                {
                    Id = table.Column<bool>(nullable: false),
                    Value = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Abouts", x => x.Id);
                });

            migrationBuilder.Sql(@"CREATE FUNCTION CheckSizeTableAbouts()
                                   RETURNS INT
                                   AS
                                   BEGIN
	                                   DECLARE @size INT
	                                   SELECT @size = COUNT(*)
	                                   FROM Abouts
	                                   RETURN @size
                                   END");
            migrationBuilder.Sql("ALTER TABLE Abouts ADD CONSTRAINT CK_ABOUTS_SIZE CHECK (dbo.CheckSizeTableAbouts() = 1)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Abouts");
            migrationBuilder.Sql("DROP FUNCTION dbo.CheckSizeTableAbouts");
        }
    }
}
