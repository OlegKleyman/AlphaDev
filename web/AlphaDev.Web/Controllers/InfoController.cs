using System.Threading.Tasks;
using AlphaDev.BlogServices.Core;
using AlphaDev.Optional.Extensions;
using AlphaDev.Web.Core;
using AlphaDev.Web.Models;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Optional;
using Optional.Async;

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
        public async Task<IActionResult> About()
        {
            IActionResult GetAboutView(string value) => View(nameof(About), value);
            return await _aboutService.GetAboutDetailsAsync()
                                      .MapAsync(GetAboutView)
                                      .WithExceptionAsync(() =>
                                      {
                                          return RedirectToAction(nameof(CreateAbout))
                                                 .SomeWhen<IActionResult>(result => User.Identity.IsAuthenticated)
                                                 .WithException(() => GetAboutView("No details"))
                                                 .ValueOrException();
                                      })
                                      .GetValueOrExceptionAsync();
        }

        [Route("about/edit")]
        public async Task<IActionResult> EditAbout()
        {
            return await _aboutService.GetAboutDetailsAsync()
                                      .WithExceptionAsync(() => RedirectToAction(nameof(About)))
                                      .MapAsync(s => new AboutEditViewModel(s))
                                      .MapAsync(model => (IActionResult) View(nameof(EditAbout), model))
                                      .GetValueOrExceptionAsync();
        }

        [SaveFilter]
        [Route("about/edit")]
        [HttpPost]
        public async Task<IActionResult> EditAbout(AboutEditViewModel model)
        {
            var option = ModelState.IsValid
                                   .Some()
                                   .FilterAsync(Task.FromResult)
                                   .WithExceptionAsync(() => View(nameof(EditAbout), model));
            await option.MatchSomeAsync(b => _aboutService.EditAsync(model.Value));
            return await option.MapAsync(dictionary => (IActionResult) RedirectToAction(nameof(About)))
                               .GetValueOrExceptionAsync();
        }

        [Route("about/create")]
        public async Task<IActionResult> CreateAbout()
        {
            return await _aboutService.GetAboutDetailsAsync()
                                      .WithExceptionAsync(() =>
                                          View(nameof(CreateAbout), new AboutCreateViewModel()))
                                      .MapAsync(s => (IActionResult) RedirectToAction(nameof(EditAbout)))
                                      .GetValueOrExceptionAsync();
        }

        [SaveFilter]
        [Route("about/create")]
        [HttpPost]
        public async Task<IActionResult> CreateAbout([NotNull] AboutCreateViewModel model)
        {
            var option = ModelState.IsValid
                                   .Some()
                                   .FilterAsync(Task.FromResult)
                                   .WithExceptionAsync(() => View(nameof(CreateAbout), model));
            await option.MatchSomeAsync(b => _aboutService.CreateAsync(model.Value));
            return await option.MapAsync(b => (IActionResult) RedirectToAction(nameof(About)))
                               .GetValueOrExceptionAsync();
        }

        [AllowAnonymous]
        [Route("contact")]
        public async Task<IActionResult> Contact()
        {
            IActionResult GetContactView(string value) => View(nameof(Contact), value);
            return await _contactService.GetContactDetailsAsync()
                                        .MapAsync(GetContactView)
                                        .WithExceptionAsync(() =>
                                        {
                                            return RedirectToAction(nameof(CreateContact))
                                                   .SomeWhen<IActionResult>(result =>
                                                       User.Identity.IsAuthenticated)
                                                   .WithException(() => GetContactView("No details"))
                                                   .GetValueOrException();
                                        })
                                        .GetValueOrExceptionAsync();
        }

        [Route("contact/edit")]
        public async Task<IActionResult> EditContact()
        {
            return await _contactService.GetContactDetailsAsync()
                                        .WithExceptionAsync(() => RedirectToAction(nameof(Contact)))
                                        .MapAsync(s => new ContactEditViewModel(s))
                                        .MapAsync(model => (IActionResult) View(nameof(EditContact), model))
                                        .GetValueOrExceptionAsync();
        }

        [SaveFilter]
        [Route("contact/edit")]
        [HttpPost]
        public async Task<IActionResult> EditContact(ContactEditViewModel model)
        {
            var option = ModelState.IsValid
                                   .Some()
                                   .FilterAsync(Task.FromResult)
                                   .WithExceptionAsync(() => View(nameof(EditContact), model));
            await option.MatchSomeAsync(b => _contactService.EditAsync(model.Value));
            return await option.MapAsync(dictionary => (IActionResult) RedirectToAction(nameof(Contact)))
                               .GetValueOrExceptionAsync();
        }

        [Route("contact/create")]
        public async Task<IActionResult> CreateContact()
        {
            return await _contactService.GetContactDetailsAsync()
                                        .WithExceptionAsync(() =>
                                            View(nameof(CreateContact), new ContactCreateViewModel()))
                                        .MapAsync(s => (IActionResult) RedirectToAction(nameof(EditContact)))
                                        .GetValueOrExceptionAsync();
        }

        [SaveFilter]
        [Route("contact/create")]
        [HttpPost]
        public async Task<IActionResult> CreateContact([NotNull] ContactCreateViewModel model)
        {
            var option = ModelState.IsValid
                                   .Some()
                                   .FilterAsync(Task.FromResult)
                                   .WithExceptionAsync(() => View(nameof(CreateContact), model));
            await option.MatchSomeAsync(b => _contactService.CreateAsync(model.Value));
            return await option.MapAsync(dictionary => (IActionResult) RedirectToAction(nameof(Contact)))
                               .GetValueOrExceptionAsync();
        }
    }
}