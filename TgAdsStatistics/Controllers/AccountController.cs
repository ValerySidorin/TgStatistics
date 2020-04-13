using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TgAdsStatistics.Logger;
using TgAdsStatistics.Models;
using TgAdsStatistics.Models.ViewModels;

namespace TgAdsStatistics.Controllers
{
    public class AccountController : Controller
    {
        private readonly CustomLoggerManager customLoggerManager;
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;
        private readonly ApplicationContext db;
        public AccountController(CustomLoggerManager customLoggerManager, UserManager<User> userManager, SignInManager<User> signInManager, ApplicationContext db)
        {
            this.customLoggerManager = customLoggerManager;
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.db = db;
        }

        [HttpGet]
        public IActionResult Register()
        {
            Log log = customLoggerManager.CreateLog();
            db.Logs.Add(log);
            db.SaveChanges();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            Log log = customLoggerManager.CreateLog();
            db.Logs.Add(log);
            db.SaveChanges();
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
            Log log = customLoggerManager.CreateLog();
            db.Logs.Add(log);
            db.SaveChanges();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            Log log = customLoggerManager.CreateLog();
            db.Logs.Add(log);
            db.SaveChanges();
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
            Log log = customLoggerManager.CreateLog();
            db.Logs.Add(log);
            db.SaveChanges();
            await signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }
    }
}