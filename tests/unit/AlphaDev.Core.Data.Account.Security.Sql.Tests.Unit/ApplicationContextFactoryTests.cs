using System.Collections.Generic;
using System.Data.SqlClient;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace AlphaDev.Core.Data.Account.Security.Sql.Tests.Unit
{
    public class ApplicationContextFactoryTests
    {
        private ApplicationContextFactory GetApplicationContextFactory(IConfigurationRoot config)
        {
            return new ApplicationContextFactory(config);
        }

        [Fact]
        public void CreateShouldReturnContextFactoryWithConnectionStringIfOnePresent()
        {
            var builder = new ConfigurationBuilder().AddInMemoryCollection(
                new[] {new KeyValuePair<string, string>("connectionStrings:AlphaDevSecurity", "Data Source=(Test);")});

            var factory = GetApplicationContextFactory(builder.Build());

            // ReSharper disable once AssignNullToNotNullAttribute - null is allowed with this method, resharper is
            // bonkers.
            factory.CreateDbContext(null).Database.GetDbConnection().Should().BeOfType<SqlConnection>().Subject
                .ConnectionString
                .Should().BeEquivalentTo("Data Source=(Test);");
        }

        [Fact]
        public void CreateShouldReturnContextFactoryWithDefaultConnectionStringIfConfigIsNull()
        {
            var factory = GetApplicationContextFactory(null);

            // ReSharper disable once AssignNullToNotNullAttribute - null is allowed with this method, resharper is
            // bonkers.
            factory.CreateDbContext(null).Database.GetDbConnection().Should().BeOfType<SqlConnection>().Subject
                .ConnectionString
                .Should().BeEquivalentTo(@"Data Source=(LocalDB)\v11.0;");
        }

        [Fact]
        public void CreateShouldReturnContextFactoryWithDefaultConnectionStringIfNoConfigIsPassed()
        {
            var factory = new ApplicationContextFactory();

            // ReSharper disable once AssignNullToNotNullAttribute - null is allowed with this method, resharper is
            // bonkers.
            factory.CreateDbContext(null).Database.GetDbConnection().Should().BeOfType<SqlConnection>().Subject
                .ConnectionString
                .Should().BeEquivalentTo(@"Data Source=(LocalDB)\v11.0;");
        }

        [Fact]
        public void CreateShouldReturnContextFactoryWithDefaultConnectionStringIfNonePresentInConfig()
        {
            var builder = new ConfigurationBuilder();

            var factory = GetApplicationContextFactory(builder.Build());

            // ReSharper disable once AssignNullToNotNullAttribute - null is allowed with this method, resharper is
            // bonkers.
            factory.CreateDbContext(null).Database.GetDbConnection().Should().BeOfType<SqlConnection>().Subject
                .ConnectionString
                .Should().BeEquivalentTo(@"Data Source=(LocalDB)\v11.0;");
        }
    }
}