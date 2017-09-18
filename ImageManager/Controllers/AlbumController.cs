using ImageManager.Services;
using Microsoft.AspNetCore.Mvc;

namespace ImageManager.Controllers
{
    public class AlbumController : Controller
    {
        private readonly AlbumService _albumService;

        public AlbumController(AlbumService albumService)
        {
            _albumService = albumService;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}