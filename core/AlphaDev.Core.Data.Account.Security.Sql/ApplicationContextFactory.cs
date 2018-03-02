using AlphaDev.Core.Data.Account.Security.Sql.Contexts;
using AlphaDev.Core.Data.Account.Security.Sql.Entities;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace AlphaDev.Core.Data.Account.Security.Sql
{
    public class ApplicationContextFactory : IDesignTimeDbContextFactory<ApplicationContext>
    {
        private readonly IConfigurationRoot _config;

        public ApplicationContextFactory()
        {
        }

        public ApplicationContextFactory(IConfigurationRoot config)
        {
            _config = config;
        }

        public ApplicationContext CreateDbContext(string[] args)
        {
            return new ApplicationContext(
                _config?.GetConnectionString("AlphaDevSecurity") ?? @"Data Source=(LocalDB)\v11.0;");
        }
    }
}