using AlphaDev.Core;
using AlphaDev.Core.Extensions;
using AlphaDev.Web.Models;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc;
using Optional;

namespace AlphaDev.Web.Controllers
{
    [Route("info")]
    public class InfoController : Controller
    {
        private readonly IInformationService _informationService;

        public InfoController([NotNull] IInformationService informationService)
        {
            _informationService = informationService;
        }

        [Route("about")]
        public ViewResult About()
        {
            return View(nameof(About), _informationService.GetAboutDetails().ValueOr(() => "No details"));
        }

        [Route("about/edit")]
        public ViewResult EditAbout()
        {
            return View(nameof(EditAbout),
                new AboutEditorViewModel(_informationService.GetAboutDetails().ValueOr(() => string.Empty)));
        }

        [Route("about/edit")]
        [HttpPost]
        public IActionResult EditAbout(AboutEditorViewModel model)
        {
            return ModelState.SomeWhen(dictionary => dictionary.IsValid)
                .MapToAction(dictionary => _informationService.Edit(model.Value))
                .Map(dictionary => (IActionResult) RedirectToAction(nameof(About)))
                .ValueOr(() => View(nameof(EditAbout), model));
        }
    }
}