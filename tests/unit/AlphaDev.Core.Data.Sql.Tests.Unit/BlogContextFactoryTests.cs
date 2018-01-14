using System.Collections.Generic;
using System.Data.SqlClient;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace AlphaDev.Core.Data.Sql.Tests.Unit
{
    public class BlogContextFactoryTests
    {
        private BlogContextFactory GetBlogContextFactory(IConfigurationRoot config)
        {
            return new BlogContextFactory(config);
        }

        [Fact]
        public void CreateShouldReturnContextFactoryWithConnectionStringIfOnePresent()
        {
            var builder = new ConfigurationBuilder().AddInMemoryCollection(
                new[] {new KeyValuePair<string, string>("connectionStrings:AlphaDevDefault", "Data Source=(Test);")});

            var factory = GetBlogContextFactory(builder.Build());

            // ReSharper disable once AssignNullToNotNullAttribute - null is allowed with this method, resharper is
            // bonkers.
            factory.CreateDbContext(null).Database.GetDbConnection().Should().BeOfType<SqlConnection>().Subject.ConnectionString
                .ShouldBeEquivalentTo("Data Source=(Test);");
        }

        [Fact]
        public void CreateShouldReturnContextFactoryWithDefaultConnectionStringIfConfigIsNull()
        {
            var factory = GetBlogContextFactory(null);

            // ReSharper disable once AssignNullToNotNullAttribute - null is allowed with this method, resharper is
            // bonkers.
            factory.CreateDbContext(null).Database.GetDbConnection().Should().BeOfType<SqlConnection>().Subject.ConnectionString
                .ShouldBeEquivalentTo(@"Data Source=(LocalDB)\v11.0;");
        }

        [Fact]
        public void CreateShouldReturnContextFactoryWithDefaultConnectionStringIfNoConfigIsPassed()
        {
            var factory = new BlogContextFactory();

            // ReSharper disable once AssignNullToNotNullAttribute - null is allowed with this method, resharper is
            // bonkers.
            factory.CreateDbContext(null).Database.GetDbConnection().Should().BeOfType<SqlConnection>().Subject.ConnectionString
                .ShouldBeEquivalentTo(@"Data Source=(LocalDB)\v11.0;");
        }

        [Fact]
        public void CreateShouldReturnContextFactoryWithDefaultConnectionStringIfNonePresentInConfig()
        {
            var builder = new ConfigurationBuilder();

            var factory = GetBlogContextFactory(builder.Build());

            // ReSharper disable once AssignNullToNotNullAttribute - null is allowed with this method, resharper is
            // bonkers.
            factory.CreateDbContext(null).Database.GetDbConnection().Should().BeOfType<SqlConnection>().Subject.ConnectionString
                .ShouldBeEquivalentTo(@"Data Source=(LocalDB)\v11.0;");
        }
    }
}