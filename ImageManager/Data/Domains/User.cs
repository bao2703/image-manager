using System.Collections.Generic;

namespace ImageManager.Data.Domains
{
    public class User : Entity
    {
        public string Name { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public List<Category> Categories { get; set; }

        public List<Album> Albums { get; set; }

        public List<Image> Images { get; set; }
    }
}