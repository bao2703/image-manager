using System;

namespace ImageManager.Data.Domains
{
    public class Entity
    {
        public int Id { get; set; }

        public DateTime? CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }
    }
}