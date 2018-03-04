using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AlphaDev.Web.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        public ViewResult Index()
        {
            return View("Index");
        }
    }
}