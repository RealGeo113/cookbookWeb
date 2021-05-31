using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace cookbookWeb.Models
{
    public class RecipeRating
    {
        [Key]
        public long Id { get; set; }

        public long UserId { get; set; }
        public long RecipeId { get; set; }
        public Recipe Recipe { get; set; }
        public User User { get; set; }
    }
}
