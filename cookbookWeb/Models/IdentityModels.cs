using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cookbookWeb.Models
{
    public class IdentityModels
    {
        public class UserLogin : IdentityUserLogin<long> { }
        public class UserRole : IdentityUserRole<long> { }
        public class UserClaim : IdentityUserClaim<long> { }
        public class Role : IdentityRole<long> { }
        public class RoleClaim : IdentityRoleClaim<long> { }
        public class UserToken : IdentityUserToken<long> { }
    }
}
