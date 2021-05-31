using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;

namespace cookbookWeb.Models
{
    public class Recipe
    {
        public long Id { get; set; }
        [ForeignKey("Author")]
        public long AuthorId { get; set; }
        public virtual User Author { get; set; }
        public string ImagePath { get; set; }
        public string Name { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime? EditedDate { get; set; }
        public bool IsEdited { get; set; }
        public int People { get; set; }
        public int Time { get; set; }
        public string TimeFormatted
        {
            get
            {
                if (Time == 1)
                {
                    return "1 minute";
                }
                if (Time < 59)
                {
                    return string.Format("{0} minutes", Time.ToString());
                }
                else
                {
                    int hours = Time / 60;
                    int minutes = Time - (hours * 60);
                    if (minutes == 0)
                    {
                        return string.Format("{0} hours", hours.ToString());
                    }
                    else
                    {
                        if (minutes == 1)
                        {
                            return string.Format("{0} hours 1 minute", hours.ToString());
                        }
                        else
                        {
                            return string.Format("{0} hours {1} minutes", hours.ToString(), minutes.ToString());
                        }
                    }
                };
            }
        }
        public string Description { get; set; }
        [ForeignKey("Category")]
        public long CategoryId { get; set; }
        public virtual Category Category { get; set; }
        public ICollection<Ingredient> Ingredients { get; set; }
        public ICollection<Instruction> Instructions { get; set; }
        public ICollection<Comment> Comments  { get; set; }
        public ICollection<RecipeRating> Ratings { get; set; }
        public ICollection<FavoriteRecipe> FavoriteRecipes { get; set; }

        public bool IsApproved { get; set; }
    }
}
