using AlphaDev.Core;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc;

namespace AlphaDev.Web.Controllers
{
    [Route("Info")]
    public class InfoController : Controller
    {
        private readonly IInformationService _informationService;

        public InfoController([NotNull] IInformationService informationService)
        {
            _informationService = informationService;
        }

        [Route("About")]
        public ViewResult About()
        {
            return View(nameof(About), _informationService.GetAboutDetails().ValueOr(() => string.Empty));
        }
    }
}