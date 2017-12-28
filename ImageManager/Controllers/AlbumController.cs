using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ImageManager.Common;
using ImageManager.Data.Domains;
using ImageManager.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sakura.AspNetCore;

namespace ImageManager.Controllers
{
    [Authorize]
    public class AlbumController : Controller
    {
        private readonly AlbumService _albumService;
        private readonly UnitOfWork _unitOfWork;
        private readonly UserService _userService;

        public AlbumController(AlbumService albumService, UnitOfWork unitOfWork, UserService userService)
        {
            _albumService = albumService;
            _unitOfWork = unitOfWork;
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string searchString, int? categoryId, int page = 1, int pageSize = 10)
        {
            if (page <= 0 || pageSize <= 0) return NotFound();

            var user = await _userService.GetUserAsync(User);
            var model = _albumService.GetUserAlbums(user.Id);

            if (!string.IsNullOrEmpty(searchString))
                model = model.Where(x =>
                    x.Name.ToLower().Contains(searchString.ToLower()) ||
                    x.Description.ToLower().Contains(searchString.ToLower()));

            if (categoryId != null)
            {
                ViewData["categoryId"] = categoryId;
                model = model.Where(x => x.Category.Id == categoryId);
            }

            var pagedData = model.ToPagedList(pageSize, page);
            return View(pagedData);
        }

        [HttpGet]
        public async Task<IActionResult> Detail(int id)
        {
            var user = await _userService.GetUserAsync(User);
            var model = _albumService.GetUserAlbum(id, user.Id);
            return View(model);
        }

        [HttpPost]
        public async Task<string> Create(Album model)
        {
            if (!ModelState.IsValid) return "invalid";

            model.Images = new List<Image>();
            model.User = await _userService.GetUserAsync(User);
            await _albumService.AddAsync(model);
            await _unitOfWork.SaveChangesAsync();
            return Url.Action("Detail", "Album", new {model.Id});
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var model = _albumService.FindById(id);
            if (model == null) return NotFound();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Album model)
        {
            if (!ModelState.IsValid) return View(model);

            var album = _albumService.FindById(model.Id);
            album.Name = model.Name;
            album.CategoryId = model.CategoryId;
            album.Description = model.Description;

            await _unitOfWork.SaveChangesAsync();

            return RedirectToAction("Index");
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

        [HttpPost]
        public async Task<IActionResult> AddImage(int id, List<IFormFile> files)
        {
            var album = _albumService.FindById(id);
            if (album == null) return BadRequest();

            album.Images.AddRange(await UploadAsync(files));
            await _unitOfWork.SaveChangesAsync();
            return Ok();
        }

        private async Task<List<Image>> UploadAsync(IList<IFormFile> files)
        {
            var images = new List<Image>();
            foreach (var file in files)
            {
                var filePath = $"/{Constant.UploadPath}/{DateTime.Now.ToFileTime()}_{file.FileName}";
                using (var stream = new FileStream($"{Constant.RootPath}{filePath}", FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                    images.Add(new Image {Path = filePath});
                }
            }
            return images;
        }
    }
}