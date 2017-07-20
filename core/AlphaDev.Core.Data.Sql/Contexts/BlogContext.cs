using AlphaDev.Core.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace AlphaDev.Core.Data.Sql.Contexts
{
    public sealed class BlogContext : Data.Contexts.BlogContext
    {
        private readonly string _connectionString;

        public BlogContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(
                _connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var entity = modelBuilder.Entity<Blog>();

            entity.HasKey(blog => blog.Id);
            entity.Property(blog => blog.Created).HasDefaultValueSql("GETUTCDATE()").ValueGeneratedOnAdd();
            entity.Property(blog => blog.Modified);
            entity.Property(blog => blog.Content).IsRequired();
            entity.Property(blog => blog.Title).IsRequired();
        }
    }
}