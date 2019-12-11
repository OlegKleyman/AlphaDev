using System.Threading.Tasks;
using AlphaDev.Core.Data.Account.Security.Sql.Entities;
using AlphaDev.Web.Models;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Optional;
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
        public IActionResult Login([CanBeNull] LoginViewModel model, [CanBeNull] string? returnUrl = null)
        {
            return ModelState.IsValid.SomeWhen(b => b)
                             .Map(b => _signInManager
                                       .PasswordSignInAsync(model?.Username, model?.Password, false, false)
                                       .GetAwaiter()
                                       .GetResult())
                             .Filter(signInResult => signInResult == SignInResult.Success)
                             .Map<IActionResult>(signInResult => Redirect(returnUrl ?? "/"))
                             .Match(actionResult => actionResult,
                                 () =>
                                 {
                                     ModelState.AddModelError(string.Empty, "Invalid login");
                                     return View("Login", model);
                                 });
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction("Index", "Default");
        }
    }
}