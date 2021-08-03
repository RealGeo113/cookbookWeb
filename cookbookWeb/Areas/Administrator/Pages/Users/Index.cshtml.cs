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
using cookbookWeb.Classes;

namespace cookbookWeb.Areas.Administrator.Pages.Users
{
    public class UsersModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        private readonly ILogger<UsersModel> _logger;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public UsersModel(ILogger<UsersModel> logger, ApplicationDbContext db, UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _logger = logger;
            _db = db;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public PaginatedList<User> Users { get; set; }
        public string IdSort { get; set; }
        public string UserNameSort { get; set;}
        public string EmailSort { get; set; }
        public async void OnGet(int? p)
        {
            Users = await PaginatedList<User>.CreateAsync(_db.Users.ToList() , p ?? 1, 32);
        }
    }
}
