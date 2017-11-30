using ImageManager.Data;
using ImageManager.Data.Domains;

namespace ImageManager.Services
{
    public class CategoryService : Service<Category>
    {
        public CategoryService(NeptuneContext context) : base(context)
        {
        }
    }
}