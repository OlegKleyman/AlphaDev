using AlphaDev.Core.Data.Sql.Contexts;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace AlphaDev.Core.Data.Sql
{
    public class BlogContextFactory : IDesignTimeDbContextFactory<BlogContext>
    {
        private readonly IConfigurationRoot _config;

        public BlogContextFactory()
        {
        }

        public BlogContextFactory(IConfigurationRoot config)
        {
            _config = config;
        }

        public BlogContext CreateDbContext(string[] args)
        {
            return new BlogContext(
                _config?.GetConnectionString("AlphaDevDefault") ?? @"Data Source=(LocalDB)\v11.0;");
        }
    }
}