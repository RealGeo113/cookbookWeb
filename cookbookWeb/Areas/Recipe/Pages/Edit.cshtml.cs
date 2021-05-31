using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using cookbookWeb.Data;
using cookbookWeb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace cookbookWeb.Areas.Recipes.Pages.Recipe
{
    [Authorize(Roles = "SuperAdmin, Admin, Moderator")]
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<User> _userManager;
        private readonly ILogger<EditModel> _logger;
        private readonly IWebHostEnvironment _environment;

        public Models.Recipe Recipe { get; set; }
        public ICollection<Category> Categories { get; set; }
        public int IngredientsCount { get; set; }
        public int InstructionCount { get; set; }
        [BindProperty]
        public InputModel Input { get; set; }
        [BindProperty]
        //[Required(ErrorMessage = "Please enter recipe category")]
        public string Category { get; set; }
        [BindProperty]
        public ICollection<InstructionInput> Instructions { get; set; }

        public class InstructionInput
        {
            public string Content { get; set; }
            public IFormFile Image { get; set; }
            public string ImagePath { get; set; }
        }

        [BindProperty]
        public ICollection<Ingredient> Ingredients { get; set; }
        public class InputModel
        {
            public long RecipeId { get; set; }
            public string RecipeImagePath { get; set; }
            //[Required(ErrorMessage = "Please enter recipe image.")]
            public IFormFile Image { get; set; }
            //[Required(ErrorMessage = "Please enter recipe name.")]
            public string Name { get; set; }

            //[Required(ErrorMessage = "Please enter time. ")]
            public int Time { get; set; }
            //[Required(ErrorMessage = "Please enter people. ")]
            public int People { get; set; }
            //[Required(ErrorMessage = "Please enter recipe description.")]
            public string Description { get; set; }
        }

        public EditModel(ILogger<EditModel> logger, ApplicationDbContext db, IWebHostEnvironment environment, UserManager<User> userManager)
        {
            _logger = logger;
            _userManager = userManager;
            _db = db;
            _environment = environment;
        }
        public async Task<IActionResult> OnGetAsync(long Id)
        {
            if (ModelState.IsValid) {
                
                if (Id == 0)
                {
                    return RedirectToPage("/Error");
                }
                else
                {
                    Recipe = _db.Recipes.Include(r => r.Category)
                                        .Include(r => r.Instructions)
                                        .Include(r => r.Ingredients)
                                        .Where(r => r.Id == Id)
                                        .SingleOrDefault();
                    if(Recipe == null)
                    {
                        return NotFound($"Unable to load recipe with Id: '{Id}'.");
                    }
                    User currentUser = _db.Users.Where(r => r.Id == int.Parse(_userManager.GetUserId(User))).SingleOrDefault();
                    var userRoles = await _userManager.GetRolesAsync(currentUser);
                    if (Recipe.AuthorId != long.Parse(_userManager.GetUserId(User)) || (!userRoles.Contains("Admin") || !userRoles.Contains("SuperAdmin")))
                    {
                        return RedirectToPage("/Recipe", new { Id = Recipe.Id });
                    }

                    Categories = _db.Categories.ToList();
                    IngredientsCount = Recipe.Ingredients.Count;
                    InstructionCount = Recipe.Instructions.Count;
                }
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                Recipe = _db.Recipes.Include(r => r.Category)
                                        .Include(r => r.Instructions)
                                        .Include(r => r.Ingredients)
                                        .Where(r => r.Id == Input.RecipeId)
                                        .SingleOrDefault();

                Recipe.Name = Input.Name;
                Recipe.Time = Input.Time;
                Recipe.People = Input.People;
                Recipe.Description = Input.Description;
                Recipe.Ingredients = Ingredients;
                Recipe.Category = _db.Categories.Where(r => r.Name == Category).SingleOrDefault();
                Recipe.Instructions = new List<Instruction>();
                Recipe.EditedDate = DateTime.UtcNow;
                Recipe.IsEdited = true;

                var instructionsList = _db.Instructions.Where(i => i.Recipe == Recipe).ToList();
                foreach(Instruction item in instructionsList)
                {
                    _db.Instructions.Remove(item);
                }

                string directory = Path.Combine(_environment.WebRootPath, "Images", Input.Name);
                if (Input.Image != null)
                {
                    Directory.CreateDirectory(directory);
                    string FileName = Guid.NewGuid() + Path.GetExtension(Input.Image.FileName);
                    string File = Path.Combine(directory, FileName);
                    using (var fileStream = new FileStream(File, FileMode.Create))
                    {
                        await Input.Image.CopyToAsync(fileStream);
                    }

                    Recipe.ImagePath = "/Images/" + Recipe.Name + "/" + FileName;
                }

                foreach (InstructionInput instruction in Instructions)
                {
                    Instruction instruction1 = new Instruction();
                    instruction1.Content = instruction.Content;


                    
                    if (instruction.Image != null)
                    {
                        string file = Path.Combine(directory, "Images");
                        Directory.CreateDirectory(file);
                        string instructionFileName = Guid.NewGuid() + Path.GetExtension(instruction.Image.FileName);
                        string instructionFile = Path.Combine(file, instructionFileName);
                        using (var fileStream = new FileStream(instructionFile, FileMode.Create))
                        {
                            await instruction.Image.CopyToAsync(fileStream);
                        }
                        instruction1.ImagePath = "/Images/" + Recipe.Name + "/Images/" + instructionFileName;
                    }
                    else if(instruction.ImagePath != null){
                        instruction1.ImagePath = instruction.ImagePath;
                    }
                    Recipe.Instructions.Add(instruction1);
                }

                _db.SaveChanges();

                return RedirectToPage("/Index", new {area = "Recipe", Id = Recipe.Id });
            }
            return Page();
        }
    }
}
