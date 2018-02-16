using System.Collections.Generic;
using System.Linq;
using AlphaDev.Core.Data.Contexts;
using Microsoft.EntityFrameworkCore;
using Optional;

namespace AlphaDev.Core
{
    public class BlogService : IBlogService
    {
        private readonly BlogContext _context;

        public BlogService(BlogContext context)
        {
            _context = context;
        }

        public BlogBase GetLatest()
        {
            var targetBlog = _context.Blogs.OrderByDescending(blog => blog.Created).FirstOrDefault();

            return targetBlog != null
                ? new Blog(targetBlog.Id,
                    targetBlog.Title ?? string.Empty,
                    targetBlog.Content ?? string.Empty,
                    new Dates(targetBlog.Created, targetBlog.Modified.ToOption()))
                : BlogBase.Empty;
        }

        public IEnumerable<BlogBase> GetAll()
        {
            return _context.Blogs.SomeNotNull().Match(blogs => blogs.Select(targetBlog => new Blog(targetBlog.Id,
                targetBlog.Title ?? string.Empty,
                targetBlog.Content ?? string.Empty,
                new Dates(targetBlog.Created, targetBlog.Modified.ToOption()))), Enumerable.Empty<Blog>);
        }
    }
}