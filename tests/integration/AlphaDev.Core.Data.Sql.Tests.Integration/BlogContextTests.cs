using System;
using System.Collections.Generic;
using System.Text;

namespace AlphaDev.Core.Data.Sql.Tests.Integration
{
    using System.Data.SqlClient;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Reflection;

    using AlphaDev.Core.Data.Sql.Contexts;

    using FluentAssertions;

    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Infrastructure;
    using Microsoft.Extensions.Configuration;

    using Xunit;

    public class BlogContextTests : IDisposable
    {
        private readonly string connectionString;

        public BlogContextTests()
        {
            var configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("connectionstrings.json", false, true).Build();

            connectionString = configuration.GetConnectionString("integration");

            var optionsBuilder = new DbContextOptionsBuilder();
            var options = optionsBuilder.UseSqlServer(connectionString).Options;

            using (var context = new DbContext(options))
            {
                context.Database.EnsureCreated();
            }
        }

        private void SeedBlogs()
        {
            var optionsBuilder = new DbContextOptionsBuilder();
            var options = optionsBuilder.UseSqlServer(connectionString).Options;

            using (var context = new DbContext(options))
            {
                Enumerable.Range(1, 10).ToList().ForEach(
                    i => context.Database.ExecuteSqlCommand($"INSERT INTO Blogs(Created, Content, Title) VALUES('1/{i}/2015', 'test {i}', 'Title {i}')"));
            }
        }

        [Fact]
        public void BlogsShouldReturnBlogs()
        {
            using (var context = GetBlogContext())
            {
                var blogs = context.Blogs.ToList();

                var blogsDictionary = blogs.Select(
                    blog => blog.GetType().GetProperties()
                        .ToDictionary(x => x.Name, x => x.GetGetMethod()?.Invoke(blog, null)));

                GetTable("Blogs").ShouldBeEquivalentTo(blogsDictionary);
            }
        }

        private BlogContext GetBlogContext()
        {
            var context = new BlogContext(connectionString);
            context.Database.Migrate();

            SeedBlogs();

            return context;
        }

        private IEnumerable<IDictionary<string, object>> GetTable(string tableName)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                using (var command = new SqlCommand($"SELECT * FROM {tableName}", connection))
                {
                    connection.Open();

                    var reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        yield return Enumerable.Range(0, reader.FieldCount)
                            .ToDictionary(reader.GetName, reader.GetValue);
                    }
                }
            }
        }

        public void Dispose()
        {
            var optionsBuilder = new DbContextOptionsBuilder();
            var options = optionsBuilder.UseSqlServer(connectionString).Options;

            using (var context = new DbContext(options))
            {
                context.Database.EnsureDeleted();
            }
        }
    }
}
