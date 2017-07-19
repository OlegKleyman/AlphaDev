﻿using System.Linq;
using AlphaDev.Core.Data.Contexts;
using Optional;

namespace AppDev.Core
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
                ? new Blog(
                    targetBlog.Title ?? string.Empty,
                    targetBlog.Content ?? string.Empty,
                    new Dates(targetBlog.Created, targetBlog.Modified.ToOption()))
                : BlogBase.Empty;
        }
    }
}