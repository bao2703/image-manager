using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using ImageManager.Common;
using ImageManager.Data.Domains;
using ImageManager.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ImageManager.Controllers
{
    [Authorize]
    public class AlbumController : Controller
    {
        private readonly AlbumService _albumService;
        private readonly UnitOfWork _unitOfWork;
        private readonly UserManager<User> _userManager;

        public AlbumController(AlbumService albumService, UserManager<User> userManager, UnitOfWork unitOfWork)
        {
            _albumService = albumService;
            _userManager = userManager;
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            var model = _albumService.GetUserAlbums(user.Id);
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Detail(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            var model = _albumService.GetUserAlbum(id, user.Id);
            return View(model);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Album model, List<IFormFile> files)
        {
            if (!ModelState.IsValid)
                return View(model);

            model.Images = new List<Image>();
            foreach (var file in files)
            {
                var filePath = $"/{Constant.UploadPath}/{DateTime.Now.ToFileTime()}_{file.FileName}";
                using (var stream = new FileStream($"{Constant.RootPath}{filePath}", FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                    model.Images.Add(new Image {Path = filePath});
                }
            }
            model.User = await _userManager.GetUserAsync(User);
            await _albumService.AddAsync(model);
            await _unitOfWork.SaveChangesAsync();
            return RedirectToAction("Detail", "Album", new {model.Id});
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int[] selectedItems)
        {
            foreach (var item in selectedItems)
            {
                var album = _albumService.FindById(item);
                album.Images.ForEach(x =>
                {
                    var path = $"{Constant.RootPath}/{x.Path}";
                    if (System.IO.File.Exists(path))
                        System.IO.File.Delete(path);
                });
                _albumService.Remove(album);
            }
            await _unitOfWork.SaveChangesAsync();
            return Ok();
        }
    }
}