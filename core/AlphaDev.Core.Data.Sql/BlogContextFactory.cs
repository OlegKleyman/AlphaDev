namespace AlphaDev.Core.Data.Sql
{
    using System;
    using System.IO;

    using AlphaDev.Core.Data.Sql.Contexts;

    using Microsoft.EntityFrameworkCore.Infrastructure;
    using Microsoft.Extensions.Configuration;

    public class BlogContextFactory : IDbContextFactory<BlogContext>
    {
        public BlogContext Create(DbContextFactoryOptions options)
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("connectionstrings.json", true, true).Build();

            return new BlogContext(builder.GetConnectionString("AlphaDevDefault") ?? "Data Source=(LocalDB)\\v11.0;AttachDbFilename=integration_data.mdf;Integrated Security=True;MultipleActiveResultSets=true");
        }
    }
}
