using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cookbookWeb.Models
{
    public class Role : IdentityRole<long>
    {
        public Role() : base() { }
        public Role(string roleName) : base(roleName) { }
    }
}
