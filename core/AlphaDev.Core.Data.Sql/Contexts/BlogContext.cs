namespace AlphaDev.Core.Data.Sql.Contexts
{
    using System;

    using AlphaDev.Core.Data.Entties;

    using Microsoft.EntityFrameworkCore;

    public sealed class BlogContext : Data.Contexts.BlogContext
    {
        private readonly string connectionString;

        public BlogContext(string connectionString) => this.connectionString = connectionString;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) => optionsBuilder.UseSqlServer(
            connectionString);

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