using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using AlphaDev.Core.Data.Entities;
using AlphaDev.Core.Data.Sql.Contexts;
using AlphaDev.Test.Integration.Core.Extensions;
using Microsoft.EntityFrameworkCore;

namespace AlphaDev.Web.Tests.Integration.Fixtures
{
    public class DatabaseFixture : IDisposable
    {
        public static Blog DefaultBlog => new Blog
        {
            Content = "Content integration test2.",
            Title = "Title integration test2.",
            Created = new DateTime(2017, 2, 1),
            Modified = new DateTime(2017, 7, 8)
        };

        public static Blog[] DefaultBlogs => new[]
        {
            new Blog
            {
                Content = "Content integration test1.",
                Title = "Title integration test1.",
                Created = new DateTime(2016, 1, 1),
                Modified = new DateTime(2016,2,1)
            },
            new Blog
            {
                Content = "Content integration test2.",
                Title = "Title integration test2.",
                Created = new DateTime(2017, 2, 1),
                Modified = new DateTime(2017, 7, 12)
            }
        };

        public DatabaseFixture()
        {
            ConnectionString =
                $@"Data Source=(LocalDB)\MSSQLLocalDB;Integrated Security=True;MultipleActiveResultSets=true;Database={
                        Guid.NewGuid()
                    };";

            BlogContext = new BlogContext(
                string.Format(
                    CultureInfo.InvariantCulture,
                    ConnectionString,
                    Directory.GetCurrentDirectory()));

            BlogContext.Database.EnsureDeleted();
            BlogContext.Database.Migrate();
        }

        public BlogContext BlogContext { get; }
        public string ConnectionString { get; }

        public void Dispose()
        {
            BlogContext.Database.EnsureDeleted();
            BlogContext.Dispose();
        }

        public void ResetDatabase()
        {
            BlogContext.DetachAll();
            BlogContext.Database.EnsureDeleted();
        }

        public void Add<TContext,  TEntity>(TContext context, params TEntity[] entities) where TContext : Microsoft.EntityFrameworkCore.DbContext where TEntity : class
        {
            context.AddRange(entities);
            context.SaveChanges();
        }
    }
}