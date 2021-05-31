using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using cookbookWeb.Data;
using cookbookWeb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace cookbookWeb.Pages
{
    [AllowAnonymous]
    public class RecipesModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<User> _userManager;
        private readonly ILogger<RecipesModel> _logger;

        public RecipesModel(ILogger<RecipesModel> logger, ApplicationDbContext db, UserManager<User> userManager)
        {
            _logger = logger;
            _db = db;
            _userManager = userManager;
        }
        public ICollection<Recipe> Recipes { get; set; }
        public async Task<IActionResult> OnGet(string? category)
        {
            if(category != null)
            {
                if (category == "Favorites")
                {
                    List<Recipe> recipes = new List<Recipe>();
                    ICollection<FavoriteRecipe> favoriteRecipes = await _db.FavoriteRecipes.Include(fr => fr.Recipe).ThenInclude(r => r.Author).Where(fr => fr.UserId == long.Parse(_userManager.GetUserId(User))).ToListAsync();
                    foreach (FavoriteRecipe item in favoriteRecipes)
                    {
                        recipes.Add(item.Recipe);
                    }
                    Recipes = recipes;
                }
                else
                {
                    Recipes = await _db.Recipes.Include(c => c.Category).Include(u => u.Author).Take(16).Where(r => r.Category.Name == category).ToListAsync();
                }
            }
            else
            {
                Recipes = await _db.Recipes.Include(c => c.Category).Include(u => u.Author).Take(16).ToListAsync();
            }
            
            return Page();
        }
    }
}
