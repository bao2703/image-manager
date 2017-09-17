using ImageManager.Data;
using ImageManager.Data.Domains;

namespace ImageManager.Services
{
    public class UserService : Service<User>
    {
        public UserService(NeptuneContext context) : base(context)
        {
        }
    }
}