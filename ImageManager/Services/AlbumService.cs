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
            return DbSet.Include(x => x.Images).Include(x => x.Category).SingleOrDefault(x => x.Id == (int) id);
        }

        public override IEnumerable<Album> GetAll()
        {
            return DbSet.Include(x => x.Images).Include(x => x.Category);
        }

        public IEnumerable<Album> GetUserAlbums(string userId)
        {
            return DbSet.Include(x => x.Images).Include(x => x.Category).Where(x => x.User.Id == userId).ToList();
        }

        public Album GetUserAlbum(int id, string userId)
        {
            return GetUserAlbums(userId).SingleOrDefault(x => x.Id == id);
        }
    }
}