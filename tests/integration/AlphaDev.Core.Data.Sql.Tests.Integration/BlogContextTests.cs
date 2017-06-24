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

    using AlphaDev.Core.Data.Entties;
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
                    i => context.Database.ExecuteSqlCommand($"INSERT INTO Blogs(Content, Title) VALUES('test {i}', 'Title {i}')"));
            }
        }

        [Fact]
        public void BlogsShouldReturnBlogs()
        {
            using (var context = GetBlogContext())
            {
                var blogs = context.Blogs;

                var blogsDictionary = blogs.Select(
                    blog => blog.GetType().GetProperties()
                        .ToDictionary(x => x.Name, x => x.GetGetMethod().Invoke(blog, null)));

                GetTable("Blogs").ShouldBeEquivalentTo(blogsDictionary);
            }
        }

        [Fact]
        public void BlogsShouldDeleteBlogs()
        {
            using (var context = GetBlogContext())
            {
                context.Blogs.RemoveRange(context.Blogs);
                context.SaveChanges();

                GetTable("Blogs").Should().BeEmpty();
            }
        }

        [Fact]
        public void BlogsShouldUpdateBlogs()
        {
            using (var context = GetBlogContext())
            {
                var targetBlog = context.Blogs.First();

                targetBlog.Title = "Updated Title";
                targetBlog.Content = "Updated Content";

                context.SaveChanges();

                GetTable("Blogs").First().ShouldBeEquivalentTo(
                    targetBlog.GetType().GetProperties().ToDictionary(
                        x => x.Name,
                        x => x.GetGetMethod().Invoke(targetBlog, null)));
            }
        }

        [Fact]
        public void BlogsShouldAddBlogWithCurrentDate()
        {
            using (var context = GetBlogContext())
            {
                var blog = new Blog
                               {
                                   Content = string.Empty,
                                   Title = string.Empty
                               };

                context.Blogs.Add(blog);

                context.SaveChanges();

                GetTable("Blogs").Last()["Created"].Should().BeOfType<DateTime>().Which.Should()
                    .BeCloseTo(DateTime.UtcNow, 1000).And.Subject.ShouldBeEquivalentTo(blog.Created);
            }
        }

        [Fact]
        public void BlogsShouldAddBlog()
        {
            using (var context = GetBlogContext())
            {
                var blog = new Blog
                               {
                                   Content = string.Empty,
                                   Title = string.Empty
                               };

                context.Blogs.Add(blog);

                context.SaveChanges();

                GetTable("Blogs").Should().HaveCount(11);
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
                            .ToDictionary(reader.GetName, i =>
                                {
                                    var value = reader.GetValue(i);
                                    return value == DBNull.Value ? null : value;
                                });
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
