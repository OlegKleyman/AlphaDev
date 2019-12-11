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
        private readonly IContactService _contactService;

        public InfoController([NotNull] IAboutService aboutService, [NotNull] IContactService contactService)
        {
            _aboutService = aboutService;
            _contactService = contactService;
        }

        [AllowAnonymous]
        [Route("about")]
        public IActionResult About()
        {
            IActionResult GetAboutView(string value) => View(nameof(About), value);

            return _aboutService.GetAboutDetails()
                                .Map(s => GetAboutView(s).Some())
                                .ValueOr(() =>
                                    RedirectToAction(nameof(CreateAbout))
                                        .SomeWhen<IActionResult>(result => User.Identity.IsAuthenticated))
                                .ValueOr(() => GetAboutView("No details"));
        }

        [Route("about/edit")]
        public IActionResult EditAbout()
        {
            return _aboutService.GetAboutDetails()
                                .Map(s => new AboutEditViewModel(s))
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

        [Route("about/create")]
        [HttpPost]
        public IActionResult CreateAbout([NotNull] AboutCreateViewModel model)
        {
            return ModelState.IsValid.SomeWhen(b => b)
                             .MapToAction(b => _aboutService.Create(model.Value))
                             .Map<IActionResult>(b => RedirectToAction(nameof(About)))
                             .ValueOr(() => View(nameof(CreateAbout), model));
        }

        [AllowAnonymous]
        [Route("contact")]
        public IActionResult Contact()
        {
            IActionResult GetContactView(string value) => View(nameof(Contact), value);

            return _contactService.GetDetails()
                                  .Map(s => GetContactView(s).Some())
                                  .ValueOr(() =>
                                      RedirectToAction(nameof(CreateContact))
                                          .SomeWhen<IActionResult>(result => User.Identity.IsAuthenticated))
                                  .ValueOr(() => GetContactView("No details"));
        }

        [Route("contact/edit")]
        public IActionResult EditContact()
        {
            return _contactService.GetDetails()
                                  .Map(s => new ContactEditViewModel(s))
                                  .Map<IActionResult>(model => View(nameof(EditContact), model))
                                  .ValueOr(() => RedirectToAction(nameof(Contact)));
        }

        [Route("contact/edit")]
        [HttpPost]
        public IActionResult EditContact(ContactEditViewModel model)
        {
            return ModelState.SomeWhen(dictionary => dictionary.IsValid)
                             .MapToAction(dictionary => _contactService.Edit(model.Value))
                             .Map<IActionResult>(dictionary => RedirectToAction(nameof(Contact)))
                             .ValueOr(() => View(nameof(EditContact), model));
        }

        [Route("contact/create")]
        public IActionResult CreateContact()
        {
            return _contactService.GetDetails()
                                  .Map<IActionResult>(s => RedirectToAction(nameof(EditContact)))
                                  .ValueOr(() => View(nameof(CreateContact), new ContactCreateViewModel()));
        }

        [Route("contact/create")]
        [HttpPost]
        public IActionResult CreateContact([NotNull] ContactCreateViewModel model)
        {
            return ModelState.IsValid.SomeWhen(b => b)
                             .MapToAction(b => _contactService.Create(model.Value))
                             .Map<IActionResult>(b => RedirectToAction(nameof(Contact)))
                             .ValueOr(() => View(nameof(CreateContact), model));
        }
    }
}