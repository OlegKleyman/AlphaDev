using AlphaDev.Core;
using AlphaDev.Web.Models;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc;

namespace AlphaDev.Web.Controllers
{
    [Route("/")]
    [Route("default")]
    public class DefaultController : Controller
    {
        private readonly IBlogService _blogService;

        public DefaultController([NotNull] IBlogService blogService) => _blogService = blogService;

        public ViewResult Index()
        {
            var view = _blogService.GetLatest()
                                   .Map(blogBase => new BlogViewModel(
                                       blogBase.Id,
                                       blogBase.Title,
                                       blogBase.Content,
                                       new DatesViewModel(blogBase.Dates.Created, blogBase.Dates.Modified)));

            return View(nameof(Index), view.ValueOr(() => BlogViewModel.Welcome));
        }

        [Route("error/{status}")]
        public IActionResult Error(int status)
        {
            var error = new ErrorModel(status, "An error occurred while processing your request.");
            return View(nameof(Error), error);
        }
    }
}