using AlphaDev.Core.Data.Account.Security.Sql.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AlphaDev.Core.Data.Account.Security.Sql.Contexts
{
    public class ApplicationContext : IdentityDbContext<User>
    {
        private readonly string _connectionString;

        public ApplicationContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        protected sealed override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(
                _connectionString);
        }
    }
}