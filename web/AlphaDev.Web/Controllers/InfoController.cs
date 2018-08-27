using Microsoft.AspNetCore.Mvc;

namespace AlphaDev.Web.Controllers
{
    public class InfoController : Controller
    {
        public IActionResult About()
        {
            return View();
        }
    }
}