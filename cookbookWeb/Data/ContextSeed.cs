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

            
            if(!roleManager.RoleExistsAsync("SuperAdmin").Result){
                await roleManager.CreateAsync(new Role("SuperAdmin"));
            }

            if(!roleManager.RoleExistsAsync("Admin").Result){
                await roleManager.CreateAsync(new Role("Admin"));
            }

            if(!roleManager.RoleExistsAsync("Moderator").Result){
                await roleManager.CreateAsync(new Role("Moderator"));
            }

            if(!roleManager.RoleExistsAsync("Basic").Result){
                await roleManager.CreateAsync(new Role("Basic"));
            }
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
                    await userManager.AddToRoleAsync(defaultUser, "SuperAdmin");
                    await userManager.AddToRoleAsync(defaultUser, "Admin");
                    await userManager.AddToRoleAsync(defaultUser, "Moderator");
                    await userManager.AddToRoleAsync(defaultUser, "Basic");
                }

            }
        }

        public static Task SeedCategories(ApplicationDbContext db){
            var categories = db.Categories.ToList();

            if (db.Categories.ToList().Count == 0){
                db.Categories.Add(new Category{Name = "Breakfast"});
                db.Categories.Add(new Category{Name = "Lunch"});
                db.Categories.Add(new Category{Name = "Dinner"});
                db.Categories.Add(new Category{Name = "Dessert"});
            }

            db.SaveChanges();
            return Task.CompletedTask;
        }
    }
}
