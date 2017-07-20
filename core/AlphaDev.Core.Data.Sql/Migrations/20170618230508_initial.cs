using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AlphaDev.Core.Data.Sql.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                "Blogs",
                table => new
                {
                    Id = table.Column<int>(nullable: false).Annotation(
                        "SqlServer:ValueGenerationStrategy",
                        SqlServerValueGenerationStrategy.IdentityColumn),
                    Content = table.Column<string>(nullable: false),
                    Created =
                    table.Column<DateTime>(nullable: false, defaultValueSql: "GETUTCDATE()"),
                    Title = table.Column<string>(nullable: false)
                },
                constraints: table => { table.PrimaryKey("PK_Blogs", x => x.Id); });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable("Blogs");
        }
    }
}