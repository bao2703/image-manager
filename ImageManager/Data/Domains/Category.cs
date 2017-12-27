using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ImageManager.Data.Domains
{
    public class Category : Entity
    {
        [Required]
        public string Name { get; set; }

        public List<Album> Albums { get; set; }
    }
}