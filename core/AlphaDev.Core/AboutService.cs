using System;
using AlphaDev.Core.Data.Entities;
using AlphaDev.Optional.Extensions;
using Microsoft.EntityFrameworkCore;
using Optional;
using Optional.Collections;

namespace AlphaDev.Core
{
    public class AboutService : IAboutService
    {
        private readonly DbSet<About> _abouts;
        
        public AboutService(DbSet<About> abouts) => _abouts = abouts;

        public Option<string> GetAboutDetails()
        {
            return _abouts.SingleOrNone().Map(about => about.Value).FilterNotNull();
        }

        public void Edit(string value)
        {
            _abouts.SingleOrNone()
                   .Match(contact => contact.Value = value,
                       () => throw new InvalidOperationException("About not found."));
        }

        public void Create(string value)
        {
            _abouts.Add(new About
            {
                Value = value
            });
        }
    }
}