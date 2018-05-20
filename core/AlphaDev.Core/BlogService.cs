using System;
using System.Collections.Generic;
using System.Linq;
using AlphaDev.Core.Data.Contexts;
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

        public BlogService(BlogContext context, IDateProvider dateProvider)
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

        public BlogBase Add(BlogBase blog)
        {
            var entity = new Data.Entities.Blog
            {
                Title = blog.Title,
                Content = blog.Content
            };

            var entry = _context.Blogs.Add(entity);
            _context.SaveChanges();

            if (entry.State != EntityState.Unchanged) throw new InvalidOperationException("Unable to save changes");

            return new Blog(entity.Id, entity.Title, entity.Content,
                new Dates(entity.Created, entity.Modified.ToOption()));
        }

        public void Delete(int id)
        {
            _context.Blogs.Find(id)
                .SomeNotNull()
                .MatchNoneContinue(() => throw new InvalidOperationException($"Blog ID {id} not found"))
                .Map(blog => _context.Blogs.Remove(blog))
                .MatchSomeContinue(blog => _context.SaveChanges())
                .Filter(entry => entry.State == EntityState.Detached)
                .MatchNone(() => throw new InvalidOperationException("Unable to delete"));
        }

        public void Edit(int id, Action<BlogEditArguments> edit)
        {
            _context.Blogs.Find(id)
                .SomeNotNull()
                .MatchNoneContinue(() => throw new InvalidOperationException($"Blog with ID {id} was not found"))
                .Map(blog => new {Blog = blog, Arguments = new BlogEditArguments()})
                .MatchSomeContinue(match =>
                {
                    edit(match.Arguments);
                    match.Blog.Content = match.Arguments.Content;
                    match.Blog.Title = match.Arguments.Title;
                    match.Blog.Modified = _dateProvider.UtcNow;
                    _context.SaveChanges();
                })
                .Filter(match => _context.Entry(match.Blog).State == EntityState.Unchanged)
                .MatchNone(() => throw new InvalidOperationException("Inconsistent change count on update"));
        }
    }
}