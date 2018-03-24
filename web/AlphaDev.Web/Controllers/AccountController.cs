using System.Threading.Tasks;
using AlphaDev.Core.Data.Account.Security.Sql.Entities;
using AlphaDev.Web.Models;
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

        public AccountController(SignInManager<User> signInManager)
        {
            _signInManager = signInManager;
        }

        public ViewResult Login(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View("Login", new LoginViewModel());
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            var signIn = await _signInManager.PasswordSignInAsync(model.Username, model.Password, false, false);

            return ModelState.SomeWhen(dictionary => dictionary.IsValid && signIn == SignInResult.Success).Match(
                dictionary => Redirect(returnUrl ?? "/"), () =>
                {
                    ModelState.AddModelError(string.Empty, "Invalid login");
                    return (IActionResult) View("Login", model);
                });
        }
    }
}