using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace cookbookWeb.Models
{
    public class Comment
    {
        [Key]
        public long Id { get; set; }
        [ForeignKey("Author")]
        public long AuthorId { get; set; }
        public User Author { get; set; }
        [ForeignKey("Recipe")]
        public long RecipeId { get; set; }
        public Recipe Recipe { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime? EditedDate { get; set; }
        public string Content { get; set; }

        public bool IsEdited { get; set; }
    }
}
