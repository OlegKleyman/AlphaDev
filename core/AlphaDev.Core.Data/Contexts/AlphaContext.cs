using Microsoft.EntityFrameworkCore;

namespace AlphaDev.Core.Data.Contexts
{
    public abstract class AlphaContext : DbContext
    {
        protected abstract override void OnConfiguring(DbContextOptionsBuilder optionsBuilder);
        protected abstract override void OnModelCreating(ModelBuilder modelBuilder);
    }
}