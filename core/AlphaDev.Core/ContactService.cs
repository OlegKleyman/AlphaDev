using AlphaDev.Core.Data;
using AlphaDev.Core.Data.Contexts;
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
                return context.Contact.SomeNotNull().Map(about => about.Value).NotNull();
            }
        }
    }
}