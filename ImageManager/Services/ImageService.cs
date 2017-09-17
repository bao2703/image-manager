using ImageManager.Data;
using ImageManager.Data.Domains;

namespace ImageManager.Services
{
    public class ImageService : Service<Image>
    {
        public ImageService(NeptuneContext context) : base(context)
        {
        }
    }
}