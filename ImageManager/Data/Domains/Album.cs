using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ImageManager.Data.Domains
{
    public class Album : Entity
    {
        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        public User User { get; set; }

        public List<Image> Images { get; set; }
    }
}