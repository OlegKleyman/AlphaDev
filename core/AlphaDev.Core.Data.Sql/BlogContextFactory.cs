namespace AlphaDev.Core.Data.Sql
{
    using System;
    using System.IO;

    using AlphaDev.Core.Data.Sql.Contexts;

    using Microsoft.EntityFrameworkCore.Infrastructure;
    using Microsoft.Extensions.Configuration;

    public class BlogContextFactory : IDbContextFactory<BlogContext>
    {
        private readonly IConfigurationRoot config;

        public BlogContextFactory() { }

        public BlogContextFactory(IConfigurationRoot config) => this.config = config;

        public BlogContext Create(DbContextFactoryOptions options) => new BlogContext(
            config?.GetConnectionString("AlphaDevDefault") ?? @"Data Source=(LocalDB)\v11.0;");
    }
}
