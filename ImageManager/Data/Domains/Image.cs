using System.ComponentModel.DataAnnotations;

namespace ImageManager.Data.Domains
{
    public class Image : Entity
    {
        public string Name { get; set; }

        public string Path { get; set; }

        public string Description { get; set; }

        public int AlbumId { get; set; }

        public Album Album { get; set; }
    }
}