using AlphaDev.Core.Data.Contexts;
using AlphaDev.Core.Data.Entities;
using AlphaDev.Core.Data.Support;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

namespace AlphaDev.Core.Data.Sql.Contexts
{
    public class BlogContext : Data.Contexts.BlogContext
    {
        public BlogContext(Configurer configurer) : base(configurer)
        {
        }

        protected sealed override void OnModelCreating([NotNull] ModelBuilder modelBuilder)
        {
            var entity = modelBuilder.Entity<Blog>();

            entity.HasKey(blog => blog.Id);
            entity.Property(blog => blog.Created).HasDefaultValueSql("GETUTCDATE()").ValueGeneratedOnAdd();
            entity.Property(blog => blog.Modified);
            entity.Property(blog => blog.Content).IsRequired();
            entity.Property(blog => blog.Title).IsRequired();
        }
    }
}