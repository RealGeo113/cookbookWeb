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
    public class RecipeModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        private readonly ILogger<RecipeModel> _logger;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        public RecipeModel(ILogger<RecipeModel> logger, ApplicationDbContext db, UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _logger = logger;
            _db = db;
            _userManager = userManager;
            _signInManager = signInManager;
        }
        [BindProperty]
        public string RecipeId { get; set; }
        public Recipe Recipe { get; set; }
        public FavoriteRecipe FavoriteRecipe {get; set; }
        public User CurrentUser { get; set; }
        public ICollection<Comment> Comments { get; set; }
        public async Task<IActionResult> OnGet(long Id)
        {
            if (ModelState.IsValid)
            {
                Recipe = _db.Recipes.Include(r => r.Category)
                                .Include(r => r.Author)
                                .Include(r => r.Instructions)
                                .Include(r => r.Ingredients)
                                .Where(r => r.Id == Id).SingleOrDefault();
                if (_signInManager.IsSignedIn(User))
                {
                    CurrentUser = _db.Users.Where(u => u.Id == long.Parse(_userManager.GetUserId(User))).SingleOrDefault();
                    FavoriteRecipe = _db.FavoriteRecipes.Where(fr => fr.RecipeId == Recipe.Id).Where(fr => fr.UserId == CurrentUser.Id).SingleOrDefault();
                }
                if (Recipe != null)
                {
                    Comments = _db.Comments.Include(c => c.Author).Where(c => c.RecipeId == Recipe.Id).OrderByDescending(c => c.CreationDate).ToList();
                    Recipe.Views++;
                    await _db.SaveChangesAsync();
                    return Page();
                }
                return NotFound($"Unable to load recipe with Id: '{Id}'.");
            }
            return Page();
        }

        public async Task<IActionResult> OnPostComment(string content, string recipeId)
        {
            Comment Comment = new Comment
            {
                Author = await _db.Users.Where(u => u.Id == long.Parse(_userManager.GetUserId(User))).SingleOrDefaultAsync(),
                Recipe = await _db.Recipes.Where(r => r.Id == long.Parse(recipeId)).SingleOrDefaultAsync(),
                CreationDate = DateTime.UtcNow,
                Content = content
            };

            await _db.Comments.AddAsync(Comment);
            
            await _db.SaveChangesAsync();

            return new JsonResult(Comment.Id);
        }

        public async Task<IActionResult> OnPostDeleteComment(string commentId)
        {
            Comment comment = await _db.Comments.FindAsync(long.Parse(commentId));

            _db.Comments.Remove(comment);

            await _db.SaveChangesAsync();
            return new JsonResult(true);
        }

        public async Task<IActionResult> OnPostFavorite(string recipeId)
        {
            FavoriteRecipe favoriteRecipe = new FavoriteRecipe
            {
                User = await _db.Users.Where(u => u.Id == long.Parse(_userManager.GetUserId(User))).SingleOrDefaultAsync(),
                Recipe = await _db.Recipes.Where(r => r.Id == long.Parse(recipeId)).SingleOrDefaultAsync(),
                AddedDate = DateTime.Now
            };

            FavoriteRecipe recipe = _db.FavoriteRecipes.Where(fr => fr.Recipe == favoriteRecipe.Recipe).Where(fr => fr.User == favoriteRecipe.User).SingleOrDefault();

            if (recipe != null)
            {
                _db.FavoriteRecipes.Remove(recipe);
            }
            else
            {
                _db.FavoriteRecipes.Add(favoriteRecipe);
            }
            await _db.SaveChangesAsync();

            return new JsonResult(true);
        }

        public async Task<IActionResult> OnPostApprove(string recipeId)
        {
            Recipe recipe = await _db.Recipes.Where(r => r.Id == long.Parse(recipeId)).SingleOrDefaultAsync();

            if (recipe.IsApproved)
            {
                recipe.IsApproved = false;
            }
            else
            {
                recipe.IsApproved = true;
            }

            await _db.SaveChangesAsync();
            return new JsonResult(true);
        }

        public async Task<IActionResult> OnPostDelete(string recipeId)
        {
            Recipe recipe = await _db.Recipes.Where(r => r.Id == long.Parse(recipeId)).SingleOrDefaultAsync();
            _db.Recipes.Remove(recipe);
            await _db.SaveChangesAsync();
            return new JsonResult(true);
        }
    }
}
