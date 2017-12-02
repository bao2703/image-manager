﻿using System;
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
        private readonly UserService _userService;

        public AdminController(ImageService imageService, UserService userService, CategoryService categoryService)
        {
            _imageService = imageService;
            _userService = userService;
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var user = await _userService.GetUserAsync(User);

            if (user.Role != Role.Admin)
                return NotFound();

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
        public IActionResult Categories()
        {
            var model = _categoryService.GetAll();
            return View(model.ToList());
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