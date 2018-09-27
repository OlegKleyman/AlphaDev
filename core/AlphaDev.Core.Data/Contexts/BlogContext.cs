using AlphaDev.Core.Data.Entities;
using AlphaDev.Core.Data.Support;
using Microsoft.EntityFrameworkCore;

namespace AlphaDev.Core.Data.Contexts
{
    public abstract class BlogContext : AlphaContext
    {
        protected BlogContext(Configurer configurer) : base(configurer)
        {
        }

        public DbSet<Blog> Blogs { get; set; }
    }
}