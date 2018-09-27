using AlphaDev.Core.Data.Sql.Contexts;
using AlphaDev.Core.Data.Support;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using NSubstitute;

namespace AlphaDev.Core.Data.Sql.Tests.Unit.Contexts
{
    public class MockBlogContext : BlogContext
    {
        public MockBlogContext() : this(Substitute.For<Configurer>())
        {
        }

        public MockBlogContext(Configurer configurer) : base(configurer)
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