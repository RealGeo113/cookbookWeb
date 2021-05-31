using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace cookbookWeb.Models
{
    public class Ingredient
    {
        [Key]
        public long Id { get; set; }
        [ForeignKey("Recipe")]
        public long RecipeId { get; set; }
        public Recipe Recipe { get; set; }
        public string Name { get; set; }
        public int Amount { get; set; }
        public string Unit { get; set; }
    }
}
