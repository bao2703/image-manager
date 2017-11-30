using System.Linq;
using ImageManager.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ImageManager.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        private readonly ImageService _imageService;

        public AdminController(ImageService imageService)
        {
            _imageService = imageService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var images = _imageService.GetAll();
            var g = images.GroupBy(x => x.Album.Category.Name).Select(x => new ChartModel
            {
                Key = x.Key,
                Value = x.Count()
            });
            return View(g.Take(10).OrderByDescending(x => x.Value));
        }

        public class ChartModel
        {
            public string Key { get; set; }

            public int Value { get; set; }
        }
    }
}