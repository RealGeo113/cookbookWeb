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

namespace cookbookWeb.Areas.Administrator.Pages.Recipes
{
    public class RecipesModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        private readonly ILogger<RecipesModel> _logger;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public RecipesModel(ILogger<RecipesModel> logger, ApplicationDbContext db, UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _logger = logger;
            _db = db;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [BindProperty]
        public PaginatedList<Models.Recipe> Recipes { get; set; }

        public async Task OnGet(int? p)
        {
            Recipes = await PaginatedList<Models.Recipe>.CreateAsync(_db.Recipes.Include(r => r.Author).ToList(), p ?? 1, 16);
        }
    }
}
