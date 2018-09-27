using System;
using AlphaDev.Core.Data.Account.Security.Sql.Contexts;
using AlphaDev.Core.Data.Support;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NSubstitute;
using Xunit;

namespace AlphaDev.Core.Data.Account.Security.Sql.Tests.Unit.Contexts
{
    public class ApplicationContextTests
    {
        public class MockApplicationContext : ApplicationContext
        {
            public MockApplicationContext(Configurer configurer) : base(configurer)
            {
            }

            public void OnConfiguringProxy(DbContextOptionsBuilder optionsBuilder)
            {
                OnConfiguring(optionsBuilder);
            }
        }

        [Fact]
        public void ConstructorShouldInitializeApplicationContext()
        {
            Action constructor = () => new ApplicationContext(default);
            constructor.Should().NotThrow();
        }

        [Fact]
        public void OnConfiguringShouldCallConfigure()
        {
            var configurer = Substitute.For<Configurer>();
            var context = new MockApplicationContext(configurer);
            var optionsBuilder = new DbContextOptionsBuilder();
            context.OnConfiguringProxy(optionsBuilder);
            configurer.Received(1).Configure(Arg.Is(optionsBuilder));
        }
    }
}