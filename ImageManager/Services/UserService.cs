using System.Security.Claims;
using System.Threading.Tasks;
using ImageManager.Data;
using ImageManager.Data.Domains;
using Microsoft.AspNetCore.Identity;

namespace ImageManager.Services
{
    public class UserService : Service<User>
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;

        public UserService(NeptuneContext context, SignInManager<User> signInManager, UserManager<User> userManager) :
            base(context)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        public Task SignInAsync(User user)
        {
            return _signInManager.SignInAsync(user, false);
        }

        public Task<SignInResult> PasswordSignInAsync(string username, string password)
        {
            return _signInManager.PasswordSignInAsync(username, password, false, false);
        }

        public Task SignOutAsync()
        {
            return _signInManager.SignOutAsync();
        }

        public Task<IdentityResult> CreateAsync(User user, string password)
        {
            return _userManager.CreateAsync(user, password);
        }

        public Task<User> GetUserAsync(ClaimsPrincipal user)
        {
            return _userManager.GetUserAsync(user);
        }
    }
}