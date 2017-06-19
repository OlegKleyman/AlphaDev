using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AlphaDev.Core.Data.Sql.Migrations
{
    public partial class AddModified : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder) => migrationBuilder.AddColumn<DateTime>(
            name: "Modified",
            table: "Blogs",
            nullable: true);

        protected override void Down(MigrationBuilder migrationBuilder) => migrationBuilder.DropColumn(
            name: "Modified",
            table: "Blogs");
    }
}
