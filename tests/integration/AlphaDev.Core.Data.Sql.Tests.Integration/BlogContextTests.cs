using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using AlphaDev.Core.Data.Entities;
using AlphaDev.Core.Data.Sql.Contexts;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace AlphaDev.Core.Data.Sql.Tests.Integration
{
    public class BlogContextTests : IDisposable
    {
        public BlogContextTests()
        {
            var configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("connectionstrings.json", false, true).Build();

            var optionsBuilder = new DbContextOptionsBuilder();
            var connectionBuilder =
                new SqlConnectionStringBuilder(configuration.GetConnectionString("integration"))
                {
                    InitialCatalog = Guid.NewGuid().ToString()
                };

            _connectionString = connectionBuilder.ToString();
            var options = optionsBuilder.UseSqlServer(_connectionString).Options;

            using (var context = new DbContext(options))
            {
                context.Database.EnsureCreated();
            }
        }

        public void Dispose()
        {
            var optionsBuilder = new DbContextOptionsBuilder();
            var options = optionsBuilder.UseSqlServer(_connectionString).Options;

            using (var context = new DbContext(options))
            {
                context.Database.EnsureDeleted();
            }
        }

        private readonly string _connectionString;

        private void SeedBlogs()
        {
            var optionsBuilder = new DbContextOptionsBuilder();
            var options = optionsBuilder.UseSqlServer(_connectionString).Options;

            using (var context = new DbContext(options))
            {
                Enumerable.Range(1, 10).ToList().ForEach(
                    // ReSharper disable once AccessToDisposedClosure - Executes eagerly
                    i => context.Database.ExecuteSqlCommand(
                        $"INSERT INTO Blogs(Content, Title) VALUES('test {i}', 'Title {i}')"));
            }
        }

        private BlogContext GetBlogContext()
        {
            var context = new BlogContext(_connectionString);
            context.Database.Migrate();

            SeedBlogs();

            return context;
        }

        private IEnumerable<IDictionary<string, object>> GetTable(string tableName)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = new SqlCommand($"SELECT * FROM {tableName}", connection))
                {
                    connection.Open();

                    var reader = command.ExecuteReader();

                    while (reader.Read())
                        yield return Enumerable.Range(0, reader.FieldCount)
                            .ToDictionary(reader.GetName, i =>
                            {
                                var value = reader.GetValue(i);
                                return value == DBNull.Value ? null : value;
                            });
                }
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
    }
}