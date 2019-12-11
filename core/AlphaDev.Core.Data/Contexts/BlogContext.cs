using System;
using AlphaDev.Core.Data.Entities;
using AlphaDev.Core.Data.Support;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

namespace AlphaDev.Core.Data.Contexts
{
    public abstract class BlogContext : AlphaContext
    {
        private DbSet<Blog>? _blogs;

        protected BlogContext(Configurer configurer) : base(configurer)
        {
        }

        [NotNull]
        public DbSet<Blog> Blogs
        {
            get => _blogs ?? throw new InvalidOperationException($"{nameof(Blogs)} is null.");
            set => _blogs = value;
        }
    }
}