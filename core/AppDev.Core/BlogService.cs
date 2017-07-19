namespace AppDev.Core
{
    using System;
    using System.Linq;

    using AlphaDev.Core.Data.Contexts;

    using Optional;

    public class BlogService : IBlogService
    {
        private BlogContext context;

        public BlogService(BlogContext context) => this.context = context;

        public BlogBase GetLatest()
        {
            var targetBlog = context.Blogs.OrderByDescending(blog => blog.Created).FirstOrDefault();

            return targetBlog != null
                       ? new Blog(
                           targetBlog.Title ?? string.Empty,
                           targetBlog.Content ?? string.Empty,
                           new Dates(targetBlog.Created, targetBlog.Modified.ToOption()))
                       : BlogBase.Empty;
        }
    }
}