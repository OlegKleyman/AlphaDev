using AlphaDev.Core.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace AlphaDev.Core.Data.Contexts
{
    public abstract class InformationContext : AlphaContext
    {
        public DbSet<About> Abouts { get; set; }
    }
}