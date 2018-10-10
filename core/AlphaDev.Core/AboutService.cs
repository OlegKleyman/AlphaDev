using System;
using AlphaDev.Core.Data;
using AlphaDev.Core.Data.Contexts;
using AlphaDev.Core.Data.Entities;
using AlphaDev.Core.Extensions;
using JetBrains.Annotations;
using Optional;

namespace AlphaDev.Core
{
    public class AboutService : IAboutService
    {
        private readonly IContextFactory<InformationContext> _contextFactory;

        public AboutService([NotNull] IContextFactory<InformationContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public Option<string> GetAboutDetails()
        {
            using (var context = _contextFactory.Create())
            {
                return context.About.SomeNotNull().Map(about => about.Value).NotNull();
            }
        }

        public void Edit(string value)
        {
            using (var context = _contextFactory.Create())
            {
                context.UpdateAndSaveSingleOrThrow(x => x.About, about => about.Value = value);
            }
        }

        public void Create(string value)
        {
            using (var context = _contextFactory.Create())
            {
                context.AddAndSaveSingleOrThrow(x => x.Abouts, new About { Value = value });
            }
        }
    }
}