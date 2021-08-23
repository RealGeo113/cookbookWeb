using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using cookbookWeb.Data;
using cookbookWeb.Models;
using cookbookWeb.Pages;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace cookbookWeb.Areas.Administrator.Pages.Users
{
    public class UserModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        private readonly ILogger<UserModel> _logger;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IWebHostEnvironment _environment;


        public UserModel(ILogger<UserModel> logger, ApplicationDbContext db, UserManager<User> userManager, SignInManager<User> signInManager, IWebHostEnvironment environment)
        {
            _logger = logger;
            _db = db;
            _userManager = userManager;
            _signInManager = signInManager;
            _environment = environment;
        }
        
        [BindProperty]
        public User InputUser { get; set; }
        [BindProperty]
        public InputModel Input { get; set; }
        public class InputModel{
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }
            public string Role { get; set; }
            public IFormFile Image { get; set; }
        }
        
        public User DisplayedUser { get; set; }
        public void OnGet(string id)
        {
            if(id != null || id != ""){
                DisplayedUser = _db.Users.Find(long.Parse(id));
            }
        }
        public async Task<IActionResult> OnPost(){
            User user = await _db.Users.FindAsync(InputUser.Id);
            user.FirstName = InputUser.FirstName;
            user.LastName = InputUser.LastName;
            user.Email = InputUser.Email;
            user.UserName = InputUser.UserName;

            if(Input.Image != null){
                string directory = Path.Combine(_environment.WebRootPath, "Images\\Users");
                if(user.ImagePath != null){
                    new FileInfo($"{directory}//{user.ImagePath}").Delete();
                }
                Directory.CreateDirectory(directory);
                string FileName = Guid.NewGuid() + Path.GetExtension(Input.Image.FileName);
                string File = Path.Combine(directory, FileName);
                using (var fileStream = new FileStream(File, FileMode.Create))
                {
                    await Input.Image.CopyToAsync(fileStream);
                }
                 user.ImagePath = "/Images/Users/" + FileName;
            }

            await _userManager.UpdateAsync(user);

            if(Input.Password != null){
                await _userManager.ChangePasswordAsync(user, user.PasswordHash, Input.Password);
            }
            
            foreach(Role role in _db.Roles.ToList()){
                if(await _userManager.IsInRoleAsync(user, role.Name) && role.Name != "Basic"){
                    await _userManager.RemoveFromRoleAsync(user, role.Name);
                }
            }

            switch(Input.Role){
                case "SuperAdmin":
                    await _userManager.AddToRolesAsync(user, new string[]{"SuperAdmin", "Admin", "Moderator"});
                break;
                case "Admin":
                    await _userManager.AddToRolesAsync(user, new string[]{"Admin", "Moderator"});
                break;
                case "Moderator":
                    await _userManager.AddToRoleAsync(user, "Moderator");
                break;
            }
            await _userManager.UpdateSecurityStampAsync(user);

            return RedirectToPage("/Users/Index", new {area = "Administrator"});
        }
    }
}
