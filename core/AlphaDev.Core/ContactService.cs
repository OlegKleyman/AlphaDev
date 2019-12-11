using AlphaDev.Core.Data;
using AlphaDev.Core.Data.Contexts;
using AlphaDev.Core.Data.Entities;
using AlphaDev.Core.Extensions;
using JetBrains.Annotations;
using Optional;

namespace AlphaDev.Core
{
    public class ContactService : IContactService
    {
        private readonly IContextFactory<InformationContext> _contextFactory;

        public ContactService([NotNull] IContextFactory<InformationContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public Option<string> GetDetails()
        {
            using var context = _contextFactory.Create();
            return context.Contact.SomeNotNull().Map(contact => contact.Value).NotNull();
        }

        public void Edit(string value)
        {
            using var context = _contextFactory.Create();
            context.UpdateAndSaveSingleOrThrow(x => x.Contact, contact => contact.Value = value);
        }

        public void Create(string value)
        {
            using var context = _contextFactory.Create();
            context.AddAndSaveSingleOrThrow(x => x.Contacts, new Contact { Value = value });
        }
    }
}