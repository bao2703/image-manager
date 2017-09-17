using ImageManager.Data;
using ImageManager.Data.Domains;

namespace ImageManager.Services
{
    public class AlbumService : Service<Album>
    {
        public AlbumService(NeptuneContext context) : base(context)
        {
        }
    }
}