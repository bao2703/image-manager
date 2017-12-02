using System.Diagnostics;
using System.Threading.Tasks;
using ImageManager.Data.Domains;
using ImageManager.Models;
using ImageManager.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ImageManager.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly UserService _userService;

        public HomeController(UserService userService)
        {
            _userService = userService;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userService.GetUserAsync(User);

            if (user.Role == Role.Admin)
                return RedirectToAction("Index", "Admin");
            return RedirectToAction("Index", "Album");
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }
    }
}