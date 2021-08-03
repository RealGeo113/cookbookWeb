using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace cookbookWeb.Models
{
    public class User : IdentityUser<long>
    {
        [ProtectedPersonalData]
        public string FirstName { get; set; }
        [ProtectedPersonalData]
        public string LastName { get; set; }
        public string FullName { get {return FirstName + " " + LastName; } }
        public string Description { get; set; }
        public string ImagePath { get; set; }
        public ICollection<FavoriteRecipe> FavoriteRecipes { get; set; }
        public DateTime Joined { get; set; }
    }
}
