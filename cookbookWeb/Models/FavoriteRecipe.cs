using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace cookbookWeb.Models
{
    public class FavoriteRecipe
    {
        [Key]
        public long Id { get; set; }
        [ForeignKey("Recipe")]
        public long RecipeId { get; set; }
        public Recipe Recipe { get; set; }
        [ForeignKey("User")]
        public long UserId { get; set; }
        public User User { get; set; }
    }
}
