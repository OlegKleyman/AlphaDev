using AlphaDev.Core.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace AlphaDev.Core.Data.Contexts
{
    public abstract class BlogContext : DbContext
    {
        public DbSet<Blog> Blogs { get; set; }

        protected abstract override void OnConfiguring(DbContextOptionsBuilder optionsBuilder);

        protected abstract override void OnModelCreating(ModelBuilder modelBuilder);
    }
}