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
using Microsoft.AspNetCore.Authentication;

namespace cookbookWeb.Pages
{
    public class RegisterModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        private readonly ILogger<RegisterModel> _logger;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public string ReturnUrl { get; set; }
        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string EmailAddress { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The username must be at least {2} and at max {1} characters long.", MinimumLength = 5)]
            public string Username { get; set; }
            [Required]
            [StringLength(100, ErrorMessage = "The password must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmedPassword { get; set; }
        }

        public void OnGet()
        {
        }

        public RegisterModel(ILogger<RegisterModel> logger, ApplicationDbContext db, UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _logger = logger;
            _db = db;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                User user = new User { Email = Input.EmailAddress, UserName = Input.Username };
                var result = await _userManager.CreateAsync(user, Input.Password);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User {0} created account.", user.UserName);
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            
            return Page();
        }
    }
}
