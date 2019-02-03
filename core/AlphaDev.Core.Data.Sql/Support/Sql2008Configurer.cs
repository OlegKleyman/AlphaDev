using AlphaDev.Core.Data.Support;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

namespace AlphaDev.Core.Data.Sql.Support
{
    public class Sql2008Configurer : Configurer
    {
        private readonly string _connectionString;

        public Sql2008Configurer(string connectionString)
        {
            _connectionString = connectionString;
        }

        public override void Configure([NotNull] DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_connectionString, builder => builder.UseRowNumberForPaging());
        }
    }
}