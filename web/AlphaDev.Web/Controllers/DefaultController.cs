namespace AlphaDev.Web.Controllers
{
    using AppDev.Core;

    using Microsoft.AspNetCore.Mvc;

    public class DefaultController : Controller
    {
        private readonly IBlogService blogService;

        public DefaultController(IBlogService blogService) => this.blogService = blogService;

        public ViewResult Index()
        {
            var blog = blogService.GetLatest();

            return View(nameof(Index), blog);
        }
    }
}