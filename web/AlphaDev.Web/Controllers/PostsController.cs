using System;
using System.Linq;
using AlphaDev.Core;
using AlphaDev.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace AlphaDev.Web.Controllers
{
    [Route("posts")]
    public class PostsController : Controller
    {
        private readonly IBlogService _blogService;

        public PostsController(IBlogService blogService)
        {
            _blogService = blogService;
        }

        public ViewResult Index()
        {
            var blogs = _blogService.GetAll();
            var model = blogs.Select(blog => new BlogViewModel(blog.Id,
                    blog.Title,
                    blog.Content,
                    new DatesViewModel(blog.Dates.Created, blog.Dates.Modified)))
                .OrderByDescending(viewModel => viewModel.Dates.Created);

            return View(nameof(Index), model);
        }

        [Route("{id}")]
        public ActionResult Index(int id)
        {
            var blog = _blogService.Get(id);

            return blog.Map(foundBlog => new BlogViewModel(foundBlog.Id,
                foundBlog.Title,
                foundBlog.Content,
                new DatesViewModel(foundBlog.Dates.Created, foundBlog.Dates.Modified))).Match(foundBlog =>
            {
                ViewBag.Title = foundBlog.Title;
                return (ActionResult) View("Post", foundBlog);
            }, NotFound);
        }
    }
}