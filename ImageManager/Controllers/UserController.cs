using ImageManager.Services;
using Microsoft.AspNetCore.Mvc;

namespace ImageManager.Controllers
{
    public class UserController : Controller
    {
        private readonly UserService _userService;

        public UserController(UserService userService)
        {
            _userService = userService;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}