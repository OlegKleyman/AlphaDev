using AlphaDev.Core;
using AlphaDev.Core.Extensions;
using AlphaDev.Web.Models;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Optional;

namespace AlphaDev.Web.Controllers
{
    [Authorize]
    [Route("info")]
    public class InfoController : Controller
    {
        private readonly IInformationService _informationService;

        public InfoController([NotNull] IInformationService informationService)
        {
            _informationService = informationService;
        }

        [AllowAnonymous]
        [Route("about")]
        public IActionResult About()
        {
            IActionResult GetAboutView(string value) => View(nameof(About), value);
            return _informationService.GetAboutDetails().Map(s => GetAboutView(s).Some())
                .ValueOr(() =>
                    RedirectToAction(nameof(CreateAbout))
                        .SomeWhen<IActionResult>(result => User.Identity.IsAuthenticated))
                .ValueOr(() => GetAboutView("No details"));
        }

        [Route("about/edit")]
        public IActionResult EditAbout()
        {
            return _informationService.GetAboutDetails().Map(s => new AboutEditViewModel(s))
                .Map<IActionResult>(model => View(nameof(EditAbout), model))
                .ValueOr(() => RedirectToAction(nameof(About)));
        }

        [Route("about/edit")]
        [HttpPost]
        public IActionResult EditAbout(AboutEditViewModel model)
        {
            return ModelState.SomeWhen(dictionary => dictionary.IsValid)
                .MapToAction(dictionary => _informationService.Edit(model.Value))
                .Map(dictionary => (IActionResult) RedirectToAction(nameof(About)))
                .ValueOr(() => View(nameof(EditAbout), model));
        }

        [Route("about/create")]
        public IActionResult CreateAbout()
        {
            return _informationService.GetAboutDetails()
                .Map<IActionResult>(s => RedirectToAction(nameof(EditAbout)))
                .ValueOr(() => View(nameof(CreateAbout), new AboutCreateViewModel()));
        }

        // TODO add unit tests
        [Route("about/create")]
        [HttpPost]
        public IActionResult CreateAbout([NotNull] AboutCreateViewModel model)
        {
            return ModelState.IsValid.SomeWhen(b => b)
                .MapToAction(b => _informationService.Create(model.Value))
                .Map<IActionResult>(b => RedirectToAction(nameof(About)))
                .ValueOr(() => View(nameof(CreateAbout), model));
        }
    }
}