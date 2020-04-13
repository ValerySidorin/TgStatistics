using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TgAdsStatistics.Extensions;
using TgAdsStatistics.Logger;
using TgAdsStatistics.Models;
using TgAdsStatistics.Models.ViewModels;

namespace TgAdsStatistics.Controllers
{
    [Authorize(Roles = "admin")]
    public class AdminController : Controller
    {
        private readonly CustomLoggerManager customLoggerManager;
        private readonly UserManager<User> userManager;
        private readonly ApplicationContext db;


        public AdminController(CustomLoggerManager customLoggerManager, UserManager<User> userManager, ApplicationContext db)
        {
            this.customLoggerManager = customLoggerManager;
            this.userManager = userManager;
            this.db = db;
        }
        [HttpGet]
        public IActionResult Users()
        {
            Log log = customLoggerManager.CreateLog();
            db.Logs.Add(log);
            db.SaveChanges();
            return View(userManager.Users.ToList());
        }

        [HttpGet]
        public IActionResult Create() 
        {
            Log log = customLoggerManager.CreateLog();
            db.Logs.Add(log);
            db.SaveChanges();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateViewModel model)
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
                    return RedirectToAction("Users");
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
        public async Task<IActionResult> Edit(string id)
        {
            Log log = customLoggerManager.CreateLog();
            db.Logs.Add(log);
            db.SaveChanges();
            User user = await userManager.FindByIdAsync(id);

            if (user == null)
                return NotFound();

            EditViewModel model = new EditViewModel {Id = user.Id, Username = user.UserName };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditViewModel model)
        {
            Log log = customLoggerManager.CreateLog();
            db.Logs.Add(log);
            db.SaveChanges();
            if (ModelState.IsValid)
            {
                User user = await userManager.FindByNameAsync(model.Username);
                
                if (user != null)
                {
                    user.UserName = model.Username;

                    var result = await userManager.UpdateAsync(user);

                    if (result.Succeeded)
                        return RedirectToAction("Users");
                    else
                    {
                        foreach (var error in result.Errors)
                            ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            Log log = customLoggerManager.CreateLog();
            db.Logs.Add(log);
            db.SaveChanges();
            User user = await userManager.FindByIdAsync(id);

            if (user != null)
                await userManager.DeleteAsync(user);

            return RedirectToAction("Users");
        }
    }
}