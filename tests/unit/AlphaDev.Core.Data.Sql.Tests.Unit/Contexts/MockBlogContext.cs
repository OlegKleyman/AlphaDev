using AlphaDev.Core.Data.Sql.Contexts;
using AlphaDev.Core.Data.Sql.Support;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

namespace AlphaDev.Core.Data.Sql.Tests.Unit.Contexts
{
    public class MockBlogContext : BlogContext
    {
        public MockBlogContext() : this(new SqlConfigurer("Server={server};Database={database}"))
        {
        }

        public MockBlogContext(SqlConfigurer configurer) : base(configurer)
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