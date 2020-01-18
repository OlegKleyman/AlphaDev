using System.Threading.Tasks;
using AlphaDev.BlogServices.Core;
using AlphaDev.Core.Extensions;
using AlphaDev.Optional.Extensions;
using AlphaDev.Web.Models;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc;
using Optional.Async;

namespace AlphaDev.Web.Controllers
{
    [Route("/")]
    [Route("default")]
    public class DefaultController : Controller
    {
        private readonly IBlogService _blogService;

        public DefaultController([NotNull] IBlogService blogService) => _blogService = blogService;

        public async Task<ViewResult> Index()
        {
            return await _blogService.GetLatestAsync()
                                     .MapAsync(blogBase => new BlogViewModel(
                                         blogBase.Id,
                                         blogBase.Title,
                                         blogBase.Content,
                                         new DatesViewModel(blogBase.Dates.Created, blogBase.Dates.Modified)))
                                     .WithExceptionAsync(() => BlogViewModel.Welcome)
                                     .To(async option => View(nameof(Index), await option.ValueOrExceptionAsync()));
        }

        [Route("error/{status}")]
        public IActionResult Error(int status)
        {
            var error = new ErrorModel(status, "An error occurred while processing your request.");
            return View(nameof(Error), error);
        }
    }
}