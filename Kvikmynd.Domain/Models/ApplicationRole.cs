﻿using Microsoft.AspNetCore.Identity;

namespace Kvikmynd.Domain.Models
{
    public class ApplicationRole : IdentityRole<int>
    {
        public ApplicationRole() : base() { }
        public ApplicationRole(string roleName) : base(roleName) { }
    }
    
}