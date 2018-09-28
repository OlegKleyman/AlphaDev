﻿using AlphaDev.Core;
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
        private readonly IAboutService _aboutService;

        public InfoController([NotNull] IAboutService aboutService)
        {
            _aboutService = aboutService;
        }

        [AllowAnonymous]
        [Route("about")]
        public IActionResult About()
        {
            IActionResult GetAboutView(string value)
            {
                return View(nameof(About), value);
            }

            return _aboutService.GetAboutDetails().Map(s => GetAboutView(s).Some())
                .ValueOr(() =>
                    RedirectToAction(nameof(CreateAbout))
                        .SomeWhen<IActionResult>(result => User.Identity.IsAuthenticated))
                .ValueOr(() => GetAboutView("No details"));
        }

        [Route("about/edit")]
        public IActionResult EditAbout()
        {
            return _aboutService.GetAboutDetails().Map(s => new AboutEditViewModel(s))
                .Map<IActionResult>(model => View(nameof(EditAbout), model))
                .ValueOr(() => RedirectToAction(nameof(About)));
        }

        [Route("about/edit")]
        [HttpPost]
        public IActionResult EditAbout(AboutEditViewModel model)
        {
            return ModelState.SomeWhen(dictionary => dictionary.IsValid)
                .MapToAction(dictionary => _aboutService.Edit(model.Value))
                .Map(dictionary => (IActionResult) RedirectToAction(nameof(About)))
                .ValueOr(() => View(nameof(EditAbout), model));
        }

        [Route("about/create")]
        public IActionResult CreateAbout()
        {
            return _aboutService.GetAboutDetails()
                .Map<IActionResult>(s => RedirectToAction(nameof(EditAbout)))
                .ValueOr(() => View(nameof(CreateAbout), new AboutCreateViewModel()));
        }

        // TODO add unit tests
        [Route("about/create")]
        [HttpPost]
        public IActionResult CreateAbout([NotNull] AboutCreateViewModel model)
        {
            return ModelState.IsValid.SomeWhen(b => b)
                .MapToAction(b => _aboutService.Create(model.Value))
                .Map<IActionResult>(b => RedirectToAction(nameof(About)))
                .ValueOr(() => View(nameof(CreateAbout), model));
        }
    }
}