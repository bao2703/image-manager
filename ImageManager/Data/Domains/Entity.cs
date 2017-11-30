using System;

namespace ImageManager.Data.Domains
{
    public class Entity : ITimestampEntity
    {
        public int Id { get; set; }

        public DateTime? DateCreated { get; set; }

        public DateTime? DateModified { get; set; }
    }

    public interface ITimestampEntity
    {
        DateTime? DateCreated { get; set; }

        DateTime? DateModified { get; set; }
    }
}