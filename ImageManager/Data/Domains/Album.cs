using System.Collections.Generic;

namespace ImageManager.Data.Domains
{
    public class Album : Entity
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public User User { get; set; }

        public List<Image> Images { get; set; }
    }
}