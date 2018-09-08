using System;
using System.Linq;
using AlphaDev.Core.Data.Contexts;
using AlphaDev.Core.Data.Entities;
using Omego.Extensions.EnumerableExtensions;
using Omego.Extensions.Poco;
using Omego.Extensions.QueryableExtensions;
using Optional;
using Optional.Collections;

namespace AlphaDev.Core
{
    public class InformationService : IInformationService
    {
        private readonly InformationContext _context;

        public InformationService(InformationContext context)
        {
            _context = context;
        }

        public Option<string> GetAboutDetails()
        {
            return _context.Abouts.Select(about => about.Value).AsEnumerable().Select(Option.Some)
                .SingleOrDefaultOrThrow(s => true, Option.None<string>,
                    new InvalidOperationException("Multiple about information found."));
        }
    }
}