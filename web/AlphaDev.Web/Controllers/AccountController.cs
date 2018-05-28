using System.Threading.Tasks;
using AlphaDev.Core.Data.Account.Security.Sql.Entities;
using AlphaDev.Web.Models;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Omego.Extensions.OptionExtensions;
using Optional;
using Option = Optional.Option;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace AlphaDev.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<User> _signInManager;

        public AccountController([NotNull] SignInManager<User> signInManager)
        {
            _signInManager = signInManager;
        }

        public ViewResult Login([CanBeNull] string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View("Login", new LoginViewModel());
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login([CanBeNull] LoginViewModel model, [CanBeNull] string returnUrl = null)
        {
            var result = Option.None<IActionResult>();

            if (ModelState.IsValid)
            {
                result = (await _signInManager.PasswordSignInAsync(model?.Username, model?.Password, false, false))
                    .SomeWhen(signInResult => signInResult == SignInResult.Success)
                    .Map(signInResult => (IActionResult) Redirect(returnUrl ?? "/"))
                    .MatchNoneContinue(() => ModelState.AddModelError(string.Empty, "Invalid login"));
            }

            return result.ValueOr(() => View("Login", model));
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction("Index", "Default");
        }
    }
}