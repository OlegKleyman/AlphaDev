using System;
using AlphaDev.Core.Data;
using AlphaDev.Core.Data.Contexts;
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
            using (var context = _contextFactory.Create())
            {
                return context.Contact.SomeNotNull().Map(contact => contact.Value).NotNull();
            }
        }

        public void Edit(string value)
        {
            using (var context = _contextFactory.Create())
            {
                context.Contact.SomeNotNull(() =>
                        new InvalidOperationException("Contact not found."))
                    .MapToAction(contact => contact.Value = value)
                    // ReSharper disable once AccessToDisposedClosure - execution either
                    // immediate or not at all
                    .Map(contact => context.SaveChanges())
                    .Filter(changes => changes == 1,
                        () => new InvalidOperationException("Inconsistent change count."))
                    .MatchNone(exception => throw exception);
            }
        }
    }
}