using ImageManager.Services;
using Microsoft.AspNetCore.Mvc;

namespace ImageManager.Controllers
{
    public class ImageController : Controller
    {
        private readonly ImageService _imageService;

        public ImageController(ImageService imageService)
        {
            _imageService = imageService;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}