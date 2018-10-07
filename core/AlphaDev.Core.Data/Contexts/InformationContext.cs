using System.Linq;
using AlphaDev.Core.Data.Entities;
using AlphaDev.Core.Data.Support;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

namespace AlphaDev.Core.Data.Contexts
{
    public abstract class InformationContext : AlphaContext
    {
        protected InformationContext(Configurer configurer) : base(configurer)
        {
        }

        public DbSet<About> Abouts { get; set; }

        [CanBeNull]
        public About About => Abouts.SingleOrDefault();

        public DbSet<Contact> Contacts { get; set; }

        [CanBeNull]
        public Contact Contact => Contacts.SingleOrDefault();
    }
}