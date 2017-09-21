using System.Linq;
using System.Threading.Tasks;
using ImageManager.Data.Domains;
using ImageManager.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ImageManager.Controllers
{
    [Authorize]
    public class AlbumController : Controller
    {
        private readonly AlbumService _albumService;
        private readonly UserManager<User> _userManager;

        public AlbumController(AlbumService albumService, UserManager<User> userManager)
        {
            _albumService = albumService;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            var model = _albumService.GetUserAlbums(user.Id).ToList();
            return View(model);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Album model)
        {
            return View(model);
        }
    }
}