using AlphaDev.Core.Data.Entities;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

namespace AlphaDev.Core.Data.Sql.Contexts
{
    public class InformationContext : Data.Contexts.InformationContext
    {
        private readonly string _connectionString;

        public InformationContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        protected override void OnConfiguring([NotNull] DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_connectionString);
        }

        protected override void OnModelCreating([NotNull] ModelBuilder modelBuilder)
        {
            var entity = modelBuilder.Entity<About>();

            entity.HasKey(x => x.Id);
            entity.Property(x => x.Value);
            entity.Property(x => x.ChangedOn);
        }
    }
}