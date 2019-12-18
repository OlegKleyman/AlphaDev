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
using Optional.Collections;

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

        [NotNull]
        public BlogBase Add([NotNull] BlogBase blog)
        {
            return _blogs.Add(new Data.Entities.Blog
                         {
                             Title = blog.Title,
                             Content = blog.Content
                         })
                         .To(entry => new Blog(entry.Entity.Id, entry.Entity.Title, entry.Entity.Content,
                             new Dates(entry.Entity.Created, entry.Entity.Modified.ToOption())));
        }

        public void Delete(int id) => _blogs.Remove(_blogs.Find(id));

        public void Edit(int id, [NotNull] Action<BlogEditArguments> edit)
        {
            var blog = _blogs.Find(id)
                             .SomeNotNull(() => new InvalidOperationException($"Blog {id} was not found."))
                             .ValueOr(exception => throw exception);
            var arguments = new BlogEditArguments();
            edit(arguments);
            blog.Content = arguments.Content;
            blog.Title = arguments.Title;
            blog.Modified = _dateProvider.UtcNow;
        }

        public IEnumerable<BlogBase> GetOrderedByDates(int start, int count)
        {
            return _blogs.OrderByDescending(x => x.Modified)
                         .ThenByDescending(x => x.Created)
                         .Skip(start - 1)
                         .Take(count)
                         .SomeNotNull()
                         .Match(blogs => blogs.Select(targetBlog =>
                                                  new Blog(targetBlog.Id,
                                                      targetBlog.Title ?? string.Empty,
                                                      targetBlog.Content ?? string.Empty,
                                                      new Dates(targetBlog.Created, targetBlog.Modified.ToOption())))
                                              .ToArray(),
                             Enumerable.Empty<BlogBase>);
        }

        public int GetCount(int start)
        {
            return _blogs.OrderByDescending(x => x.Modified)
                         .ThenByDescending(x => x.Created)
                         .Skip(start - 1)
                         .Count();
        }
    }
}