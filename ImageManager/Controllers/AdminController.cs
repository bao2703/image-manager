using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ImageManager.Data.Domains;
using ImageManager.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ImageManager.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        private readonly CategoryService _categoryService;
        private readonly ImageService _imageService;
        private readonly UnitOfWork _unitOfWork;
        private readonly UserService _userService;

        public AdminController(ImageService imageService, UserService userService, CategoryService categoryService,
            UnitOfWork unitOfWork)
        {
            _imageService = imageService;
            _userService = userService;
            _categoryService = categoryService;
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            if (await CheckRole() == false) return NotFound();

            var images = _imageService.GetAll();
            var g = images.GroupBy(x => x.Album.Category.Name).Select(x => new ChartDataModel
            {
                Key = x.Key,
                Value = x.Count()
            }).OrderByDescending(x => x.Value);

            var users = _userService.GetAll();
            var g2 = users.GroupBy(x => new DateTime(x.DateCreated.Value.Year, x.DateCreated.Value.Month, 1)).Select(
                x => new
                {
                    x.Key,
                    Value = x.Count()
                }).OrderByDescending(x => x.Key).Select(x => new ChartDataModel
            {
                Key = x.Key.ToString("Y"),
                Value = x.Value
            });

            var model = new AdminModel
            {
                ImagesChartData = g.Take(10).ToList(),
                UsersChartData = g2.Take(10).ToList()
            };

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Categories()
        {
            if (await CheckRole() == false) return NotFound();

            var model = _categoryService.GetAll();
            return View(model.ToList());
        }

        [HttpGet]
        public async Task<IActionResult> CreateCategory()
        {
            if (await CheckRole() == false) return NotFound();

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateCategory(Category model)
        {
            if (await CheckRole() == false) return NotFound();

            if (ModelState.IsValid)
            {
                _categoryService.Add(model);
                await _unitOfWork.SaveChangesAsync();
                return RedirectToAction("Categories");
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> EditCategory(int id)
        {
            if (await CheckRole() == false) return NotFound();

            var model = _categoryService.FindById(id);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditCategory(Category model)
        {
            if (await CheckRole() == false) return NotFound();

            if (ModelState.IsValid)
            {
                var category = _categoryService.FindById(model.Id);
                category.Name = model.Name;
                _categoryService.Update(category);
                await _unitOfWork.SaveChangesAsync();
                return RedirectToAction("Categories");
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            if (await CheckRole() == false) return NotFound();

            var model = _categoryService.FindById(id);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ConfirmDeleteCategory(int id)
        {
            if (await CheckRole() == false) return NotFound();

            var model = _categoryService.FindById(id);
            _categoryService.Remove(model);
            await _unitOfWork.SaveChangesAsync();

            return RedirectToAction("Categories");
        }

        private async Task<bool> CheckRole()
        {
            var user = await _userService.GetUserAsync(User);
            if (user.Role != Role.Admin)
                return false;
            return true;
        }

        public class AdminModel
        {
            public List<ChartDataModel> ImagesChartData { get; set; }

            public List<ChartDataModel> UsersChartData { get; set; }
        }

        public class ChartDataModel
        {
            public string Key { get; set; }

            public int Value { get; set; }
        }
    }
}