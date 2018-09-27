using AlphaDev.Core.Data.Entities;
using AlphaDev.Core.Data.Support;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

namespace AlphaDev.Core.Data.Sql.Contexts
{
    public class InformationContext : Data.Contexts.InformationContext
    {
        public InformationContext(Configurer configurer) : base(configurer)
        {
        }

        protected override void OnModelCreating([NotNull] ModelBuilder modelBuilder)
        {
            var entity = modelBuilder.Entity<About>();

            entity.HasKey(x => x.Id);
            entity.Property(x => x.Value).IsRequired();
        }
    }
}