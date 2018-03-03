﻿using System;
using System.Globalization;
using System.IO;
using AlphaDev.Core.Data.Entities;
using AlphaDev.Core.Data.Sql.Contexts;
using AlphaDev.Test.Integration.Core.Extensions;
using Microsoft.EntityFrameworkCore;

namespace AlphaDev.Web.Tests.Integration.Fixtures
{
    public class BlogContextDatabaseFixture : IDisposable
    {
        public BlogContextDatabaseFixture(string connectionString)
        {
            ConnectionString = connectionString;

            BlogContext = new BlogContext(
                string.Format(
                    CultureInfo.InvariantCulture,
                    ConnectionString,
                    Directory.GetCurrentDirectory()));
            
            BlogContext.Database.Migrate();
        }

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
                Created = new DateTime(2014, 1, 1),
                Modified = new DateTime(2016, 2, 1)
            },
            new Blog
            {
                Content = "Content integration test2.",
                Title = "Title integration test2.",
                Created = new DateTime(2015, 2, 1),
                Modified = new DateTime(2017, 7, 12)
            }
        };

        public BlogContext BlogContext { get; }
        public string ConnectionString { get; }

        public void Dispose()
        {
            BlogContext.Dispose();
        }
    }
}