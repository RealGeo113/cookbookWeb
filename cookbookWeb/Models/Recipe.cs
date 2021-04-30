using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cookbookWeb.Models
{
    public class Recipe
    {
        private int _id;
        private int _authorId;
        private string _imagePath;
        private string _name;
        private DateTime _creationDate;
        private DateTime _editedDate;
        private string _description;
        private string _category;
        private int _people;
        private int _time;
        private Ingredient[] _ingredients;
        private Instruction[] _instructions;

        private bool _isApproved;
    }
}
