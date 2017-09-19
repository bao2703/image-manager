using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace ImageManager.Data.Domains
{
    public class User : IdentityUser
    {
        public string Name { get; set; }

        public List<Category> Categories { get; set; }

        public List<Album> Albums { get; set; }

        public List<Image> Images { get; set; }
    }
}