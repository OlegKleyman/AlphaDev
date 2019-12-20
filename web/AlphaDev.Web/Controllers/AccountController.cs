using System.Threading.Tasks;
using AlphaDev.Core.Data.Account.Security.Sql.Entities;
using AlphaDev.Optional.Extensions;
using AlphaDev.Web.Models;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Optional;
using Optional.Async;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace AlphaDev.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<User> _signInManager;

        public AccountController([NotNull] SignInManager<User> signInManager) => _signInManager = signInManager;

        public ViewResult Login([CanBeNull] string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View("Login", new LoginViewModel());
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login([CanBeNull] LoginViewModel model, [CanBeNull] string? returnUrl = null)
        {
            var mapAsync = await ModelState.IsValid.SomeWhen(b => b)
                                           .MapAsync(async b =>
                                               await _signInManager.PasswordSignInAsync(model?.Username,
                                                   model?.Password, false, false))
                                           .FilterAsync(signInResult => signInResult == SignInResult.Success)
                                           .MapAsync(signInResult => (IActionResult) Redirect(returnUrl ?? "/"))
                                           .WithExceptionAsync(() => View("Login", model));
            mapAsync.MatchNone(result => ModelState.AddModelError(string.Empty, "Invalid login"));
            return mapAsync.GetValueOrException();
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction("Index", "Default");
        }
    }
}