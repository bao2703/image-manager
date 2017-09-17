using System.Collections.Generic;

namespace ImageManager.Data.Domains
{
    public class Category : Entity
    {
        public string Name { get; set; }

        public User User { get; set; }

        public List<Image> Images { get; set; }

        public List<Album> Albums { get; set; }
    }
}