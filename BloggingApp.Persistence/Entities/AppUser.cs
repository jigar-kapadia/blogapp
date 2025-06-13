using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BloggingApp.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace BloggingApp.Persistence.Entities
{
    public class AppUser: IdentityUser
    {
        public bool IsActive { get; set; } = true;
        public bool IsBlocked { get; set; } = false;
        public ICollection<Post> Posts { get; set; }
    }
}