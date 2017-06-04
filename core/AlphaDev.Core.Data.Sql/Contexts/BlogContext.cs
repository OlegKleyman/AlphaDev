namespace AlphaDev.Core.Data.Sql.Contexts
{
    using Microsoft.EntityFrameworkCore;

    public sealed class BlogContext : Data.Contexts.BlogContext
    {
        private readonly string connectionString;

        public BlogContext(string connectionString) => this.connectionString = connectionString;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) => optionsBuilder.UseSqlServer(
            connectionString);

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
        }
    }
}