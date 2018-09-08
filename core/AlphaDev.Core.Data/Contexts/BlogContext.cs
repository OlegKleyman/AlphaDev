using AlphaDev.Core.Data.Entities;
using AlphaDev.Core.Data.Support;
using Microsoft.EntityFrameworkCore;

namespace AlphaDev.Core.Data.Contexts
{
    public abstract class BlogContext : AlphaContext
    {
        public DbSet<Blog> Blogs { get; set; }

        protected BlogContext(Configurer configurer) : base(configurer)
        {
        }
    }
}