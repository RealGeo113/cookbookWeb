using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
using Microsoft.Extensions.Logging;

namespace cookbookWeb.Areas.Recipes.Pages.Recipe
{
    [Authorize(Roles = "Superadmin, Admin, Moderator")]
    public class AddModel : PageModel
    {
        private readonly IWebHostEnvironment _environment;
        private readonly ApplicationDbContext _db;
        private readonly ILogger<AddModel> _logger;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        public Models.Recipe Recipe { get; set; }
        [BindProperty]
        public InputModel Input { get; set; }
        [BindProperty]
        //[Required(ErrorMessage = "Please enter recipe category")]
        public string Category { get; set; }
        public ICollection<Category> Categories { get; set; }
        [BindProperty]
        public ICollection<InstructionInput> Instructions {get; set; }
        [BindProperty]
        public ICollection<Ingredient> Ingredients { get; set; }

        public class InstructionInput
        {
            public string Content { get; set; }
            public IFormFile Image { get; set; }
        }

        public class InputModel
        {
            //[Required(ErrorMessage = "Please enter recipe image.")]
            public IFormFile Image { get; set; }
            //[Required(ErrorMessage = "Please enter recipe name.")]
            public string Name { get; set; }
            
            //[Required(ErrorMessage = "Please enter time. ")]
            public int Time { get; set; }
            //[Required(ErrorMessage = "Please enter people. ")]
            public int People { get; set; }
            [Required(ErrorMessage = "Please enter recipe description.")]
            [MaxLength(270)]
            public string Description { get; set; }
        }

        public AddModel(ILogger<AddModel> logger, ApplicationDbContext db, IWebHostEnvironment environment, UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _logger = logger;
            _db = db;
            _environment = environment;
            _userManager = userManager;
            _signInManager = signInManager;
        }
        public IActionResult OnGet()
        {
            if (ModelState.IsValid)
            {
                Categories = _db.Categories.ToList();
                return Page();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {

                Recipe = new Models.Recipe
                {
                    Name = Input.Name,
                    CreationDate = DateTime.UtcNow,
                    Time = Input.Time,
                    People = Input.People,
                    Description = Input.Description,
                    Ingredients = Ingredients,
                    Author = _db.Users.Where(r => r.Id == int.Parse(_userManager.GetUserId(User))).SingleOrDefault(),
                    Category = _db.Categories.Where(r => r.Name == Category).SingleOrDefault(),
                    Instructions = new List<Instruction>()
                };

                string directory = Path.Combine(_environment.WebRootPath, "Images" ,Input.Name);
                Directory.CreateDirectory(directory);
                string FileName = Guid.NewGuid() + Path.GetExtension(Input.Image.FileName);
                string File = Path.Combine(directory, FileName);
                using (var fileStream = new FileStream(File, FileMode.Create))
                {
                    await Input.Image.CopyToAsync(fileStream);
                }

                Recipe.ImagePath = "/Images/" + Recipe.Name + "/" + FileName;
                
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
                    Recipe.Instructions.Add(instruction1);
                }

                _db.Recipes.Add(Recipe);
                _db.SaveChanges();

                return RedirectToPage("/Index", new { area = "Recipe", Id = Recipe.Id });
            }
            return Page();
        }
    }
}
