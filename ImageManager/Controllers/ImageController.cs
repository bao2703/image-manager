using System.Threading.Tasks;
using ImageManager.Common;
using ImageManager.Data.Domains;
using ImageManager.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Sakura.AspNetCore;

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
        public async Task<IActionResult> Index(int page = 1, int pageSize = 60)
        {
            if (page <= 0 || pageSize <= 0) return NotFound();
            var user = await _userManager.GetUserAsync(User);
            var model = _imageService.GetAll(user.Id);
            var pagedData = model.ToPagedList(pageSize, page);
            ViewData["userId"] = user.Id;
            return View(pagedData);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int[] selectedItems)
        {
            foreach (var item in selectedItems)
            {
                var image = _imageService.FindById(item);
                var path = $"{Constant.RootPath}/{image.Path}";
                if (System.IO.File.Exists(path))
                    System.IO.File.Delete(path);
                _imageService.Remove(image);
            }
            await _unitOfWork.SaveChangesAsync();
            return Ok();
        }
    }
}