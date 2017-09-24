using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ImageManager.Common;
using ImageManager.Data.Domains;
using ImageManager.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ImageManager.Controllers
{
    [Authorize]
    public class ImageController : Controller
    {
        private readonly ImageService _imageService;
        private readonly UnitOfWork _unitOfWork;
        private readonly UserManager<User> _userManager;

        public ImageController(ImageService imageService, UserManager<User> userManager, UnitOfWork unitOfWork)
        {
            _imageService = imageService;
            _userManager = userManager;
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int? albumId)
        {
            var user = await _userManager.GetUserAsync(User);
            var images = _imageService.GetAll(user.Id).ToList();
            List<Image> model;
            if (albumId == null)
            {
                model = images;
                ViewData["Title"] = "My Images";
            }
            else
            {
                model = images.Where(x => x.Album.Id == albumId).ToList();
                var album = images.FirstOrDefault()?.Album;
                ViewData["Title"] = $"{album?.Name} Album";
            }

            return View(model.ToList());
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int[] selectedItems)
        {
            foreach (var item in selectedItems)
            {
                var image = _imageService.FindById(item);
                var path = $"{Constant.RootPath}/{image.Path}";
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }
                _imageService.Remove(image);
            }
            await _unitOfWork.SaveChangesAsync();
            return Ok();
        }
    }
}