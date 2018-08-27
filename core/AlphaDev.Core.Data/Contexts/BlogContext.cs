using AlphaDev.Core.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace AlphaDev.Core.Data.Contexts
{
    public abstract class BlogContext : AlphaContext
    {
        public DbSet<Blog> Blogs { get; set; }
    }
}