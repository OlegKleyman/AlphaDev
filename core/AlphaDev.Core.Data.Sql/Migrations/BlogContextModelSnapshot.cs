using System;
using AlphaDev.Core.Data.Sql.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;

namespace AlphaDev.Core.Data.Sql.Migrations
{
    [DbContext(typeof(BlogContext))]
    internal class BlogContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.2")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("AlphaDev.Core.Data.Entities.Blog", b =>
            {
                b.Property<int>("Id")
                 .ValueGeneratedOnAdd();

                b.Property<string>("Content")
                 .IsRequired();

                b.Property<DateTime>("Created")
                 .ValueGeneratedOnAdd()
                 .HasDefaultValueSql("GETUTCDATE()");

                b.Property<DateTime?>("Modified");

                b.Property<string>("Title")
                 .IsRequired();

                b.HasKey("Id");

                b.ToTable("Blogs");
            });
        }
    }
}