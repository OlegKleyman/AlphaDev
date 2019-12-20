using System;
using System.Threading.Tasks;
using AlphaDev.Core.Data.Entities;
using AlphaDev.Optional.Extensions;
using Microsoft.EntityFrameworkCore;
using Optional;

namespace AlphaDev.Core
{
    public class AboutService : IAboutService
    {
        private readonly DbSet<About> _abouts;

        public AboutService(DbSet<About> abouts) => _abouts = abouts;

        public async Task<Option<string>> GetAboutDetailsAsync()
        {
            return (await _abouts.SingleOrDefaultAsync())
                   .SomeNotNull()
                   .Map(about => about.Value);
        }

        public async Task EditAsync(string value)
        {
            (await _abouts.SingleOrDefaultAsync())
                .SomeNotNull()
                .Match(about => about.Value = value,
                    () => throw new InvalidOperationException("About not found."));
        }

        public async Task CreateAsync(string value)
        {
            await _abouts.AddAsync(new About
            {
                Value = value
            });
        }
    }
}