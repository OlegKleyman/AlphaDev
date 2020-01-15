using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AlphaDev.Core.Extensions;
using AlphaDev.Optional.Extensions;
using AlphaDev.Services;
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

        public async Task<Option<Unit, ObjectNotFoundException<BlogBase>>> DeleteAsync(int id)
        {
            _blogs.Include(blog1 => blog1.Id.ToString());
            var option = await _blogs.FindAsync(id)
                                     .SomeNotNullAsync(() =>
                                         new ObjectNotFoundException<BlogBase>((nameof(Blog.Id), id)));
            option.MatchSome(blog => _blogs.Remove(blog));
            return option.Map(blog => Unit.Value);
        }

        public async Task<Option<Unit, ObjectNotFoundException<BlogBase>>> EditAsync(int id,
            [NotNull] Action<BlogEditArguments> edit)
        {
            var option = await _blogs.FindAsync(id)
                                     .SomeNotNullAsync(() =>
                                         new ObjectNotFoundException<BlogBase>((nameof(Blog.Id), id)));
            option.MatchSome(blog =>
            {
                var arguments = new BlogEditArguments();
                edit(arguments);
                blog.Content = arguments.Content;
                blog.Title = arguments.Title;
                blog.Modified = _dateProvider.UtcNow;
            });

            return option.Map(blog => Unit.Value);
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

        public async Task<(int total, IEnumerable<BlogBase> blogs)> GetOrderedByDatesWithTotalAsync(int start,
            int count)
        {
            return await _blogs.CountAsync()
                               .SomeWhenAsync(i => i > 0, x => (x, Enumerable.Empty<BlogBase>()))
                               .MapAsync(async i => (i, await GetOrderedByDatesAsync(start, count)))
                               .GetValueOrExceptionAsync();
        }

        public async Task<int> GetCountAsync() => await _blogs.CountAsync();
    }
}