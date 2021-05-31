using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using cookbookWeb.Data;
using cookbookWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace cookbookWeb.Pages
{
    public class LoginModel : PageModel
    {
        [BindProperty]
        public InputModel Input { get; set; }
        private readonly ApplicationDbContext _db;
        private readonly ILogger<LoginModel> _logger;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public class InputModel{

            [Required(ErrorMessage = "Please enter your Username or your Email.")]
            public string UserName { get; set; }
            [Required(ErrorMessage = "Please enter your Password.")]
            public string Password { get; set; }

            public bool RememberMe { get; set; }

        }

        public LoginModel(ILogger<LoginModel> logger, ApplicationDbContext db, UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _logger = logger;
            _db = db;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(Input.UserName) ?? await _userManager.FindByEmailAsync(Input.UserName);
                var result = await _signInManager.PasswordSignInAsync(user.UserName, Input.Password, Input.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User logged in.");
                    return LocalRedirect(returnUrl);
                }
                else
                {
                    _logger.LogError("Login failure: User {0}, Password: {1}", Input.UserName, Input.Password);
                }
            }
            return Page();
        }
    }
}
