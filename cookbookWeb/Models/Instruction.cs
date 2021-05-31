using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace cookbookWeb.Models
{
    public class Instruction
    {
        public long Id { get; set; }
        public long RecipeId { get; set; }
        public Recipe Recipe { get; set; }
        public string Content { get; set; }
        public string? ImagePath { get; set; }
    }
}
