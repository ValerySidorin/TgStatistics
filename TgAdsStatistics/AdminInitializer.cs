using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TgAdsStatistics.Models;

namespace TgAdsStatistics
{
    public class AdminInitializer
    {
        public static async Task InitializeAsync(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            string adminLogin = "admin";
            string adminPassword = "Admin_228";


            if (await roleManager.FindByNameAsync("admin") == null)
                await roleManager.CreateAsync(new IdentityRole("admin"));

            if (await roleManager.FindByNameAsync("user") == null)
                await roleManager.CreateAsync(new IdentityRole("user"));

            if (await userManager.FindByNameAsync(adminLogin) == null)
            {
                User user = new User { UserName = adminLogin };
                var result = await userManager.CreateAsync(user, adminPassword);

                if (result.Succeeded)
                    await userManager.AddToRoleAsync(user, "admin");
            }
        }
    }
}
