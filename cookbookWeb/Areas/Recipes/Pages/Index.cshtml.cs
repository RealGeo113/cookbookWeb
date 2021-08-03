using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using cookbookWeb.Classes;
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
        public PaginatedList<Recipe> Recipes { get; set; }
        public string Category { get; set; }
        public ICollection<Category> Categories { get; set; }
        public int P { get; set; }
        public async Task<IActionResult> OnGet(string? category, int? p)
        {
            Categories = _db.Categories.ToList();
            
            P = p ?? 1;
            if (category != null)
            {
                Category = category.ToLower();
                if (category.ToLower() == "favorites")
                {
                    List<Recipe> recipes = new List<Recipe>();
                    ICollection<FavoriteRecipe> favoriteRecipes = await _db.FavoriteRecipes
                        .Include(fr => fr.Recipe)
                        .ThenInclude(r => r.Author)
                        .Where(fr => fr.UserId == long.Parse(_userManager.GetUserId(User)))
                        .OrderByDescending(fr => fr.AddedDate)
                        .ToListAsync();
                    
                    foreach (FavoriteRecipe item in favoriteRecipes)
                    {
                        recipes.Add(item.Recipe);
                    }
                    Recipes = await PaginatedList<Recipe>.CreateAsync(recipes, P, 16);
                }
                else
                {

                    Recipes = await PaginatedList<Recipe>.CreateAsync(
                        await _db.Recipes
                        .Include(c => c.Category)
                        .Include(u => u.Author)
                        .Where(r => r.Category.Name == category)
                        .OrderByDescending(r => r.CreationDate)
                        .ToListAsync()
                        , P, 16);
                }
            }
            else
            {
                Recipes = await PaginatedList<Recipe>.CreateAsync(
                    await _db.Recipes
                    .Include(r => r.Category)
                    .Include(r => r.Author)
                    .OrderByDescending(r => r.CreationDate)
                    .ToListAsync()
                    , P, 16);
            }
            
            return Page();
        }

        public async Task<IActionResult> OnPostCategory(string category, int? p){
            Category cat = await _db.Categories.Where(c => c.Name == category).FirstOrDefaultAsync();
            if(cat != null){
                Recipes = await PaginatedList<Recipe>
                    .CreateAsync(_db.Recipes
                        .Where(r => r.Category.Name == category)
                        .Include(r => r.Category)
                        .Include(r => r.Author)
                        .ToList(),
                     p ?? 1, 16);
                return new JsonResult(Recipes.Count());
            }

            return new JsonResult(false);
        }
    }
}
