using System;
using System.Collections.Generic;
using System.Text;

namespace AlphaDev.Core.Data.Sql.Tests.Unit
{
    using System.Data.SqlClient;
    using System.IO;

    using FluentAssertions;

    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Infrastructure;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Configuration.Memory;

    using Xunit;

    public class BlogContextFactoryTests
    {
        [Fact]
        public void CreateShouldReturnContextFactoryWithDefaultConnectionStringIfConfigIsNull()
        {
            var factory = GetBlogContextFactory(null);

            factory.Create(null).Database.GetDbConnection().Should().BeOfType<SqlConnection>().Subject.ConnectionString
                .ShouldBeEquivalentTo(@"Data Source=(LocalDB)\v11.0;");
        }

        [Fact]
        public void CreateShouldReturnContextFactoryWithDefaultConnectionStringIfNonePresentInConfig()
        {
            var builder = new ConfigurationBuilder();

            var factory = GetBlogContextFactory(builder.Build());

            factory.Create(null).Database.GetDbConnection().Should().BeOfType<SqlConnection>().Subject.ConnectionString
                .ShouldBeEquivalentTo(@"Data Source=(LocalDB)\v11.0;");
        }

        [Fact]
        public void CreateShouldReturnContextFactoryWithConnectionStringIfOnePresent()
        {
            var builder = new ConfigurationBuilder().AddInMemoryCollection(
                new[] { new KeyValuePair<string, string>("connectionStrings:AlphaDevDefault", "Data Source=(Test);") });
            
            var factory = GetBlogContextFactory(builder.Build());

            factory.Create(null).Database.GetDbConnection().Should().BeOfType<SqlConnection>().Subject.ConnectionString
                .ShouldBeEquivalentTo("Data Source=(Test);");
        }

        [Fact]
        public void CreateShouldReturnContextFactoryWithDefaultConnectionStringIfNoConfigIsPassed()
        {
            var factory = new BlogContextFactory();

            factory.Create(null).Database.GetDbConnection().Should().BeOfType<SqlConnection>().Subject.ConnectionString
                .ShouldBeEquivalentTo(@"Data Source=(LocalDB)\v11.0;");
        }

        private BlogContextFactory GetBlogContextFactory(IConfigurationRoot config) => new BlogContextFactory(config);
    }
}
