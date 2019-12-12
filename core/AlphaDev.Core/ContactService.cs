using System;
using System.Linq;
using AlphaDev.Core.Data.Entities;
using AlphaDev.Optional.Extensions;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Optional;

namespace AlphaDev.Core
{
    public class ContactService : IContactService
    {
        [NotNull] private readonly DbSet<Contact> _contacts;
        
        public ContactService([NotNull] DbSet<Contact> contacts) => _contacts = contacts;

        public Option<string> GetDetails()
        {
            return _contacts.SingleOrDefault().SomeWhenNotNull().Map(contact => contact.Value).FilterNotNull();
        }

        public void Edit(string value)
        {
            _contacts.SingleOrDefault()
                     .SomeNotNull()
                     .Match(contact => contact.Value = value,
                         () => throw new InvalidOperationException("Contact not found."));
        }

        public void Create(string value)
        {
            _contacts.Add(new Contact
            {
                Value = value
            });
        }
    }
}