using System;
using System.Linq;
using AlphaDev.Core.Data.Entities;
using AlphaDev.Core.Data.Support;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

namespace AlphaDev.Core.Data.Contexts
{
    public abstract class InformationContext : AlphaContext
    {
        private DbSet<About>? _abouts;

        private DbSet<Contact>? _contacts;

        protected InformationContext(Configurer configurer) : base(configurer)
        {
        }

        [NotNull]
        public DbSet<About> Abouts
        {
            get => _abouts ?? throw new InvalidOperationException($"{nameof(Abouts)} is null.");
            set => _abouts = value;
        }

        [CanBeNull]
        public About? About => Abouts.SingleOrDefault();

        [NotNull]
        public DbSet<Contact> Contacts
        {
            get => _contacts ?? throw new InvalidOperationException($"{nameof(Contacts)} is null.");
            set => _contacts = value;
        }

        [CanBeNull]
        public Contact? Contact => Contacts.SingleOrDefault();
    }
}