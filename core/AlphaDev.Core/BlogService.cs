using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AlphaDev.Core.Extensions;
using AlphaDev.Optional.Extensions;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Optional;
using Optional.Async;

namespace AlphaDev.Core
{
    public class BlogService : IBlogService
    {
        private readonly DbSet<Data.Entities.Blog> _blogs;
        private readonly IDateProvider _dateProvider;

        public BlogService(DbSet<Data.Entities.Blog> blogs, [NotNull] IDateProvider dateProvider)
        {
            _blogs = blogs;
            _dateProvider = dateProvider;
        }

        public async Task<Option<BlogBase>> GetLatestAsync()
        {
            var latestBlog = await _blogs.OrderByDescending(blog => blog.Created).FirstOrDefaultAsync();

            return latestBlog.SomeNotNull()
                             .Map(blog => (BlogBase) new Blog(blog.Id, blog.Title, blog.Content,
                                 new Dates(blog.Created, blog.Modified.ToOption())));
        }

        public Task<Option<BlogBase>> GetAsync(int id)
        {
            return _blogs.FindAsync(id)
                         .SomeNotNullAsync()
                         .MapAsync(blog => (BlogBase) new Blog(blog.Id, blog.Title, blog.Content,
                             new Dates(blog.Created, blog.Modified.ToOption())));
        }

        public async Task<BlogBase> AddAsync([NotNull] BlogBase blog)
        {
            return await _blogs.AddAsync(new Data.Entities.Blog
                               {
                                   Title = blog.Title,
                                   Content = blog.Content
                               })
                               .ToAsync(entry => new Blog(entry.Entity.Id, entry.Entity.Title, entry.Entity.Content,
                                   new Dates(entry.Entity.Created, entry.Entity.Modified.ToOption())));
        }

        public async Task DeleteAsync(int id)
        {
            var blog = await _blogs.FindAsync(id)
                                   .SomeNotNullAsync(() => new InvalidOperationException($"Blog {id} was not found."))
                                   .ValueOrAsync(exception => throw exception);
            _blogs.Remove(blog);
        }

        public async Task EditAsync(int id, [NotNull] Action<BlogEditArguments> edit)
        {
            var blog = await _blogs.FindAsync(id)
                                   .SomeNotNullAsync(() => new InvalidOperationException($"Blog {id} was not found."))
                                   .ValueOrAsync(exception => throw exception);
            var arguments = new BlogEditArguments();
            edit(arguments);
            blog.Content = arguments.Content;
            blog.Title = arguments.Title;
            blog.Modified = _dateProvider.UtcNow;
        }

        public async Task<IEnumerable<BlogBase>> GetOrderedByDatesAsync(int start, int count)
        {
            return await _blogs.OrderByDescending(x => x.Modified)
                               .ThenByDescending(x => x.Created)
                               .Skip(start - 1)
                               .Take(count)
                               .Select(targetBlog =>
                                   new Blog(targetBlog.Id,
                                       targetBlog.Title,
                                       targetBlog.Content,
                                       new Dates(targetBlog.Created, targetBlog.Modified.ToOption())))
                               .ToArrayAsync();
        }

        public async Task<int> GetCountAsync(int start)
        {
            return await _blogs.OrderByDescending(x => x.Modified)
                               .ThenByDescending(x => x.Created)
                               .Skip(start - 1)
                               .CountAsync();
        }
    }
}