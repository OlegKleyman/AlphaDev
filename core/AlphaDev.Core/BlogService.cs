using System;
using System.Collections.Generic;
using System.Linq;
using AlphaDev.Core.Data.Contexts;
using Optional;
using Optional.Collections;

namespace AlphaDev.Core
{
    public class BlogService : IBlogService
    {
        private readonly BlogContext _context;

        public BlogService(BlogContext context)
        {
            _context = context;
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
            throw new NotImplementedException();
        }
    }
}