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
                context.About.SomeNotNull(() =>
                        new InvalidOperationException("About not found."))
                    .MapToAction(about => about.Value = value)
                    // ReSharper disable once AccessToDisposedClosure - execution either
                    // immediate or not at all
                    .Map(about => context.SaveChanges())
                    .Filter(changes => changes == 1,
                        () => new InvalidOperationException("Inconsistent change count."))
                    .MatchNone(exception => throw exception);
            }
        }

        public void Create(string value)
        {
            using (var context = _contextFactory.Create())
            {
                context.Abouts.Add(new About { Value = value }).SomeNotNull(() =>
                        new InvalidOperationException("Unable to retrieve added entry."))
                    // ReSharper disable once AccessToDisposedClosure - execution either
                    // immediate or not at all
                    .Map(entry => context.SaveChanges())
                    .Filter(changes => changes == 1, () => new InvalidOperationException("Inconsistent change count."))
                    .MatchNone(exception => throw exception);
            }
        }
    }
}