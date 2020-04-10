using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TgAdsStatistics.Extensions;
using TgAdsStatistics.Logger;
using TgAdsStatistics.Models;
using TgAdsStatistics.Models.ViewModels;

namespace TgAdsStatistics.Controllers
{
    public class AccountController : Controller
    {
        private ApplicationContext db;
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;
        private ContextAccessor contextAccessor;

        ILoggerFactory loggerFactory = LoggerFactory.Create(builder =>
        {
            builder.ClearProviders();
        });
        ILogger logger;

        public AccountController(ApplicationContext db, UserManager<User> userManager, SignInManager<User> signInManager, ContextAccessor contextAccessor)
        {
            this.db = db;
            loggerFactory.AddFile("logger.txt");
            logger = loggerFactory.CreateLogger<AccountController>();
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.contextAccessor = contextAccessor;

        }

        [HttpGet]
        public IActionResult Register()
        {
            contextAccessor.Log(db, logger);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            contextAccessor.Log(db, logger);
            if (ModelState.IsValid)
            {
                User user = new User { UserName = model.UserName };
                var result = await userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "user");
                    await signInManager.SignInAsync(user, false);
                    return RedirectToAction("Posts", "Home");
                }
                else
                {
                    foreach (var error in result.Errors)
                        ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Login()
        {
            contextAccessor.Log(db, logger);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            contextAccessor.Log(db, logger);
            if (ModelState.IsValid)
            {
                var result = await signInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, false);

                if (result.Succeeded)
                {
                    return RedirectToAction("Posts", "Home");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Login or password are incorrect");
                }
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            contextAccessor.Log(db, logger);
            await signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }
    }
}