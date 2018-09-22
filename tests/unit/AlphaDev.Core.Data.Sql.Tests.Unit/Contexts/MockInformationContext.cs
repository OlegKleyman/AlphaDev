using AlphaDev.Core.Data.Sql.Contexts;
using AlphaDev.Core.Data.Sql.Support;
using AlphaDev.Core.Data.Support;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using NSubstitute;

namespace AlphaDev.Core.Data.Sql.Tests.Unit.Contexts
{
    internal class MockInformationContext : InformationContext
    {
        public MockInformationContext() : this(Substitute.For<Configurer>())
        {
        }

        public MockInformationContext(Configurer configurer) : base(configurer)
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