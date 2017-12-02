using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace ImageManager.Data.Domains
{
    public enum Role
    {
        None,
        Admin
    }

    public class User : IdentityUser
    {
        public string Name { get; set; }

        public Role Role { get; set; }

        public List<Album> Albums { get; set; }

        public DateTime? DateCreated { get; set; }

        public DateTime? DateModified { get; set; }
    }
}