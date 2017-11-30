using System.Collections.Generic;

namespace ImageManager.Data.Domains
{
    public class Category : Entity
    {
        public string Name { get; set; }

        public List<Album> Albums { get; set; }
    }
}