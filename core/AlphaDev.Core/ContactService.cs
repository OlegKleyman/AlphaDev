using System;
using System.Threading.Tasks;
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

        public async Task<Option<string>> GetContactDetailsAsync()
        {
            return (await _contacts.SingleOrDefaultAsync())
                   .SomeWhenNotNull()
                   .Map(contact => contact.Value);
        }

        public async Task EditAsync(string value)
        {
            (await _contacts.SingleOrDefaultAsync())
                .SomeNotNull()
                .Match(contact => contact.Value = value,
                    () => throw new InvalidOperationException("Contact not found."));
        }

        public async Task CreateAsync(string value)
        {
            await _contacts.AddAsync(new Contact
            {
                Value = value
            });
        }
    }
}