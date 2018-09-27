using System;
using AlphaDev.Core.Data.Contexts;
using AlphaDev.Core.Data.Support;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NSubstitute;
using Xunit;

namespace AlphaDev.Core.Data.Tests.Unit.Contexts
{
    public class AlphaContextTests
    {
        private TestAlphaContext GetAlphaContext(Configurer configurer)
        {
            return new TestAlphaContext(configurer);
        }

        public class TestAlphaContext : AlphaContext
        {
            public TestAlphaContext(Configurer configurer) : base(configurer)
            {
            }

            public void CallOnConfiguring(DbContextOptionsBuilder optionsBuilder)
            {
                OnConfiguring(optionsBuilder);
            }

            protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
                throw new NotImplementedException();
            }
        }

        [Fact]
        public void ConstructorShouldInitializeAlphaContext()
        {
            Action constructor = () => Substitute.For<AlphaContext>(Substitute.For<Configurer>());
            constructor.Should().NotThrow();
        }

        [Fact]
        public void OnConfiguringShouldCallConfigure()
        {
            var configurer = Substitute.For<Configurer>();
            var context = GetAlphaContext(configurer);
            var builder = new DbContextOptionsBuilder();
            context.CallOnConfiguring(builder);
            configurer.Received(1).Configure(Arg.Is(builder));
        }
    }
}