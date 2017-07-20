using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AlphaDev.Core.Data.Sql.Migrations
{
    public partial class AddModified : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                "Modified",
                "Blogs",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                "Modified",
                "Blogs");
        }
    }
}