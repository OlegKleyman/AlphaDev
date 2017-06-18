using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace AlphaDev.Core.Data.Sql.Migrations
{
    using AlphaDev.Core.Data.Entties;

    using Microsoft.EntityFrameworkCore.Migrations.Operations;

    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder) => migrationBuilder.CreateTable(
            name: "Blogs",
            columns: table => new
                                  {
                                      Id = table.Column<int>(nullable: false).Annotation(
                                          "SqlServer:ValueGenerationStrategy",
                                          SqlServerValueGenerationStrategy.IdentityColumn),
                                      Content = table.Column<string>(nullable: false),
                                      Created = table.Column<DateTime>(nullable: false, defaultValue:DateTime.UtcNow),
                                      Title = table.Column<string>(nullable: false)
                                  },
            constraints: table => { table.PrimaryKey("PK_Blogs", x => x.Id); });

        protected override void Down(MigrationBuilder migrationBuilder) => migrationBuilder.DropTable(
            name: "Blogs");
    }
}
