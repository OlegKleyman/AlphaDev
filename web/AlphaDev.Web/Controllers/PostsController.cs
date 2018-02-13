using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace AlphaDev.Web.Controllers
{
    public class PostsController : Controller
    {
        public IActionResult Index() => View(nameof(Index));

    }
}