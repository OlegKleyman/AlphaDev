using Microsoft.AspNetCore.Mvc;

namespace AlphaDev.Web.Controllers
{
    public class AdminController : Controller
    {
        public ViewResult Index()
        {
            return View("Index");
        }
    }
}