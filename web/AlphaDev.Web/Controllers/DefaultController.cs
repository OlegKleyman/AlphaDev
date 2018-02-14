using AlphaDev.Core;
using AlphaDev.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace AlphaDev.Web.Controllers
{
    public class DefaultController : Controller
    {
        private readonly IBlogService _blogService;

        public DefaultController(IBlogService blogService)
        {
            _blogService = blogService;
        }

        public ViewResult Index()
        {
            var blog = _blogService.GetLatest();
            var model = new BlogViewModel(
                blog.Id,
                blog.Title,
                blog.Content,
                new DatesViewModel(blog.Dates.Created, blog.Dates.Modified));

            return View(nameof(Index), model);
        }

        public IActionResult Error()
        {
            return View(nameof(Error));
        }
    }
}