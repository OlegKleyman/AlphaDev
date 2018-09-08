using AlphaDev.Core.Data.Support;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

namespace AlphaDev.Core.Data.Sql.Support
{
    public class SqlConfigurer : Configurer
    {
        private readonly string _connectionString;

        public SqlConfigurer(string connectionString) => _connectionString = connectionString;

        public override void Configure([NotNull] DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_connectionString);
        }
    }
}