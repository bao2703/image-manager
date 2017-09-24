using System.Collections.Generic;
using System.Linq;
using ImageManager.Data;
using ImageManager.Data.Domains;
using Microsoft.EntityFrameworkCore;

namespace ImageManager.Services
{
    public class AlbumService : Service<Album>
    {
        public AlbumService(NeptuneContext context) : base(context)
        {
        }

        public override Album FindById(object id)
        {
            return DbSet.Include(x => x.Images).SingleOrDefault(x => x.Id == (int) id);
        }

        public override IEnumerable<Album> GetAll()
        {
            return DbSet.Include(x => x.Images);
        }

        public IEnumerable<Album> GetUserAlbums(string userId)
        {
            return DbSet.Include(x => x.Images).Where(x => x.User.Id == userId);
        }
    }
}