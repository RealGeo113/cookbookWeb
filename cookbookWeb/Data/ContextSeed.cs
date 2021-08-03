using cookbookWeb.Classes;
using cookbookWeb.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cookbookWeb.Data
{
    public class ContextSeed
    {
        
        public static async Task SeedRolesAsync(UserManager<User> userManager, RoleManager<Role> roleManager)
        {
            //Seed Roles
            //roleManager.RoleExistsAsync()
            await roleManager.CreateAsync(new Role(Enums.Roles.SuperAdmin.ToString()));
            await roleManager.CreateAsync(new Role(Enums.Roles.Admin.ToString()));
            await roleManager.CreateAsync(new Role(Enums.Roles.Moderator.ToString()));
            await roleManager.CreateAsync(new Role(Enums.Roles.Basic.ToString()));
        }

        public static async Task SeedSuperAdminAsync(UserManager<User> userManager, RoleManager<Role> roleManager)
        {
            //Seed Default User
            var defaultUser = new User
            {
                UserName = "Admin",
                Email = "admin@admin.com",
                FirstName = "Grzegorz",
                LastName = "Łukaszun",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                Joined = DateTime.Now
            };
            if (userManager.Users.All(u => u.Id != defaultUser.Id))
            {
                var user = await userManager.FindByEmailAsync(defaultUser.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultUser, "Qwerty123!");
                    await userManager.AddToRoleAsync(defaultUser, Enums.Roles.Basic.ToString());
                    await userManager.AddToRoleAsync(defaultUser, Enums.Roles.Moderator.ToString());
                    await userManager.AddToRoleAsync(defaultUser, Enums.Roles.Admin.ToString());
                    await userManager.AddToRoleAsync(defaultUser, Enums.Roles.SuperAdmin.ToString());
                }

            }
        }
    }
}
