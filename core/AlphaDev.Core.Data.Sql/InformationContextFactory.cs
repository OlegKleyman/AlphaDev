using AlphaDev.Core.Data.Sql.Contexts;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace AlphaDev.Core.Data.Sql
{
    public class InformationContextFactory : IDesignTimeDbContextFactory<InformationContext>
    {
        private readonly IConfigurationRoot _config;

        public InformationContextFactory()
        {
        }

        public InformationContextFactory(IConfigurationRoot config)
        {
            _config = config;
        }

        [NotNull]
        public InformationContext CreateDbContext(string[] args)
        {
            return new InformationContext(
                _config?.GetConnectionString("AlphaDevDefault") ?? @"Data Source=(LocalDB)\v11.0;");
        }
    }
}