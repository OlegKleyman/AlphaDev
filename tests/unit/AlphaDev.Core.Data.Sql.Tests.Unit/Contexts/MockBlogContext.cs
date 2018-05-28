using AlphaDev.Core.Data.Sql.Contexts;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

namespace AlphaDev.Core.Data.Sql.Tests.Unit.Contexts
{
    public class MockBlogContext : BlogContext
    {
        public MockBlogContext() : base("Server={server};Database={database}")
        {
        }

        public MockBlogContext(string connectionString) : base(connectionString)
        {
        }

        public void OnModelCreatingProxy([NotNull] ModelBuilder modelBuilder)
        {
            OnModelCreating(modelBuilder);
        }

        public void OnConfiguringProxy([NotNull] DbContextOptionsBuilder builder)
        {
            OnConfiguring(builder);
        }
    }
}