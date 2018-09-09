using AlphaDev.Core.Data.Account.Security.Sql.Entities;
using AlphaDev.Core.Data.Support;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AlphaDev.Core.Data.Account.Security.Sql.Contexts
{
    public class ApplicationContext : IdentityDbContext<User>
    {
        private readonly Configurer _configurer;

        public ApplicationContext(Configurer configurer)
        {
            _configurer = configurer;
        }

        protected sealed override void OnConfiguring([NotNull] DbContextOptionsBuilder optionsBuilder)
        {
            _configurer.Configure(optionsBuilder);
        }
    }
}