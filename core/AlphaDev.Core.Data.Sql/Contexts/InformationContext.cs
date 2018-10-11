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
            var aboutEntity = modelBuilder.Entity<About>();
            aboutEntity.HasKey(x => x.Id);
            aboutEntity.Property(x => x.Value).IsRequired();

            var contactEntity = modelBuilder.Entity<Contact>();
            contactEntity.HasKey(x => x.Id);
            contactEntity.Property(x => x.Value).IsRequired();
        }
    }
}