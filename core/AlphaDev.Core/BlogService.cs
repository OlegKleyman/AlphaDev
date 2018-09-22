using System;
using System.Collections.Generic;
using System.Linq;
using AlphaDev.Core.Data.Contexts;
using AlphaDev.Core.Extensions;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Omego.Extensions.OptionExtensions;
using Optional;
using Optional.Collections;

namespace AlphaDev.Core
{
    public class BlogService : IBlogService
    {
        private readonly BlogContext _context;
        private readonly IDateProvider _dateProvider;

        public BlogService([NotNull] BlogContext context, [NotNull] IDateProvider dateProvider)
        {
            _context = context;
            _dateProvider = dateProvider;
        }

        public Option<BlogBase> GetLatest()
        {
            return _context.Blogs.OrderByDescending(blog => blog.Created).FirstOrNone().Map(blog =>
                (BlogBase) new Blog(blog.Id,
                    blog.Title ?? string.Empty,
                    blog.Content ?? string.Empty,
                    new Dates(blog.Created, blog.Modified.ToOption())));
        }

        public IEnumerable<BlogBase> GetAll()
        {
            return _context.Blogs.SomeNotNull().Match(blogs => blogs.Select(targetBlog => new Blog(targetBlog.Id,
                targetBlog.Title ?? string.Empty,
                targetBlog.Content ?? string.Empty,
                new Dates(targetBlog.Created, targetBlog.Modified.ToOption()))), Enumerable.Empty<Blog>);
        }

        public Option<BlogBase> Get(int id)
        {
            return _context.Blogs.Find(id).SomeNotNull().Map(blog =>
                (BlogBase) new Blog(blog.Id, blog.Title, blog.Content,
                    new Dates(blog.Created, blog.Modified.ToOption())));
        }

        [NotNull]
        public BlogBase Add([NotNull] BlogBase blog)
        {
            return new Data.Entities.Blog
                {
                    Title = blog.Title,
                    Content = blog.Content
                }.Some().Map(targetBlog => _context.Blogs.Add(targetBlog))
                .MapToAction(() => _context.SaveChanges())
                .Filter(entry => entry.State == EntityState.Unchanged)
                .WithException(() => new InvalidOperationException("Unable to save changes"))
                .Map(entry => new Blog(entry.Entity.Id, entry.Entity.Title, entry.Entity.Content,
                    new Dates(entry.Entity.Created, entry.Entity.Modified.ToOption())))
                .ValueOr(exception => throw exception);
        }

        public void Delete(int id)
        {
            _context.Blogs.Find(id)
                .SomeNotNull(() => new InvalidOperationException($"Blog ID {id} not found"))
                .Map(blog => _context.Blogs.Remove(blog))
                .MapToAction(() => _context.SaveChanges())
                .Filter(entry => entry.State == EntityState.Detached,
                    () => new InvalidOperationException("Unable to delete"))
                .MatchNone(exception => throw exception);
        }

        public void Edit(int id, Action<BlogEditArguments> edit)
        {
            _context.Blogs.Find(id)
                .SomeNotNull(() => new InvalidOperationException($"Blog with ID {id} was not found"))
                .Map(blog => new { Blog = blog, Arguments = new BlogEditArguments() })
                .MapToAction(match => edit(match.Arguments))
                .MapToAction(match =>
                {
                    match.Blog.Content = match.Arguments.Content;
                    match.Blog.Title = match.Arguments.Title;
                    match.Blog.Modified = _dateProvider.UtcNow;
                })
                .MapToAction(() => _context.SaveChanges())
                .Filter(match => _context.Entry(match.Blog).State == EntityState.Unchanged, () => new InvalidOperationException("Inconsistent change count on update"))
                .MatchNone(exception => throw exception);
        }
    }
}