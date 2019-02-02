using System;
using System.Collections.Generic;
using System.Linq;
using AlphaDev.Core.Data;
using AlphaDev.Core.Data.Contexts;
using AlphaDev.Core.Extensions;
using JetBrains.Annotations;
using Optional;
using Optional.Collections;

namespace AlphaDev.Core
{
    public class BlogService : IBlogService
    {
        [NotNull] private readonly IContextFactory<BlogContext> _contextFactory;
        private readonly IDateProvider _dateProvider;

        public BlogService([NotNull] IContextFactory<BlogContext> contextFactory, [NotNull] IDateProvider dateProvider)
        {
            _contextFactory = contextFactory;
            _dateProvider = dateProvider;
        }

        public Option<BlogBase> GetLatest()
        {
            using (var context = _contextFactory.Create())
            {
                return context.Blogs.OrderByDescending(blog => blog.Created).FirstOrNone().Map(blog =>
                    (BlogBase) new Blog(blog.Id,
                        blog.Title ?? string.Empty,
                        blog.Content ?? string.Empty,
                        new Dates(blog.Created, blog.Modified.ToOption())));
            }
        }

        public Option<BlogBase> Get(int id)
        {
            using (var context = _contextFactory.Create())
            {
                return context.Blogs.Find(id).SomeNotNull().Map(blog =>
                    (BlogBase) new Blog(blog.Id, blog.Title, blog.Content,
                        new Dates(blog.Created, blog.Modified.ToOption())));
            }
        }

        [NotNull]
        public BlogBase Add([NotNull] BlogBase blog)
        {
            using (var context = _contextFactory.Create())
            {
                return context.AddAndSaveSingleOrThrow(x => x.Blogs, new Data.Entities.Blog
                {
                    Title = blog.Title,
                    Content = blog.Content
                }).Map(entry => new Blog(entry.Entity.Id, entry.Entity.Title, entry.Entity.Content,
                    new Dates(entry.Entity.Created, entry.Entity.Modified.ToOption())));
            }
        }

        public void Delete(int id)
        {
            using (var context = _contextFactory.Create())
            {
                context.DeleteSingleOrThrow(new Data.Entities.Blog { Id = id });
            }
        }

        public void Edit(int id, Action<BlogEditArguments> edit)
        {
            using (var context = _contextFactory.Create())
            {
                context.UpdateAndSaveSingleOrThrow(x => x.Blogs.Find(id), blog =>
                {
                    var arguments = new BlogEditArguments();
                    edit(arguments);
                    blog.Content = arguments.Content;
                    blog.Title = arguments.Title;
                    blog.Modified = _dateProvider.UtcNow;
                });
            }
        }

        public IEnumerable<BlogBase> GetOrderedByDates(int start, int count)
        {
            using (var context = _contextFactory.Create())
            {
                return context.Blogs.OrderByDescending(x => x.Modified).ThenByDescending(x => x.Created)
                    .Skip(start - 1).Take(count).SomeNotNull().Match(blogs => blogs.Select(targetBlog =>
                            new Blog(targetBlog.Id,
                                targetBlog.Title ?? string.Empty,
                                targetBlog.Content ?? string.Empty,
                                new Dates(targetBlog.Created, targetBlog.Modified.ToOption()))).ToArray(),
                        Enumerable.Empty<BlogBase>);
            }
        }

        public int GetCount(int start)
        {
            using (var context = _contextFactory.Create())
            {
                return context.Blogs.OrderByDescending(x => x.Modified).ThenByDescending(x => x.Created).Skip(start - 1)
                    .Count();
            }
        }
    }
}