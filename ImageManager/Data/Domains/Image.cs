using System.ComponentModel.DataAnnotations;

namespace ImageManager.Data.Domains
{
    public class Image : Entity
    {
        [Required]
        public string Name { get; set; }

        public string Path { get; set; }

        public string Description { get; set; }

        public Album Album { get; set; }
    }
}