namespace AlphaDev.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    public class DefaultController : Controller
    {
        public IActionResult Index() => View(nameof(Index));
    }
}