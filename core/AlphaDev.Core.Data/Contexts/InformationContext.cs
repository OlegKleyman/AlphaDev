using AlphaDev.Core.Data.Entities;
using AlphaDev.Core.Data.Support;
using Microsoft.EntityFrameworkCore;

namespace AlphaDev.Core.Data.Contexts
{
    public abstract class InformationContext : AlphaContext
    {
        public DbSet<About> Abouts { get; set; }

        protected InformationContext(Configurer configurer) : base(configurer)
        {
        }
    }
}