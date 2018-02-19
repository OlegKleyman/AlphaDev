using AlphaDev.Core;
using AlphaDev.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Optional;

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
            var blog = _blogService.GetLatest().Map(blogBase => new BlogViewModel(
                blogBase.Id,
                blogBase.Title,
                blogBase.Content,
                new DatesViewModel(blogBase.Dates.Created, blogBase.Dates.Modified)));
            
            return View(nameof(Index), blog);
        }

        public IActionResult Error()
        {
            return View(nameof(Error));
        }
    }
}