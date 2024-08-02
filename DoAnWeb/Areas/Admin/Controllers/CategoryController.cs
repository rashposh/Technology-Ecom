﻿using DoAnWeb.Models;
using DoAnWeb.Models.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace DoAnWeb.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Admin")]
public class CategoryController : Controller
{
    private readonly DataContext _dataContext;

    public CategoryController(DataContext context)
    {
        _dataContext = context;
    }
    public async Task<IActionResult> Index()
    {
        return View(await _dataContext.Categories.OrderByDescending(p => p.Id).ToListAsync());
    }
    public IActionResult Create()
    {
        return View();
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CategoryModel category)
    {
        if (ModelState.IsValid)
        {
            //code them du lieu
            category.Slug = category.Name.Replace(" ", "-");
            var slug = await _dataContext.Categories.FirstOrDefaultAsync(p => p.Slug == category.Slug);
            if (slug != null)
            {
                ModelState.AddModelError("", "danh mục da co trong database");
                return View(category);
            }
            _dataContext.Add(category);
            await _dataContext.SaveChangesAsync();
            TempData["success"] = "Thêm danh sục thành công";
            return RedirectToAction("Index");
        }
        else
        {
            TempData["error"] = "Model có 1 vài thứ đang bị lỗi";
            List<string> errors = new List<string>();
            foreach (var value in ModelState.Values)
            {
                foreach (var error in value.Errors)
                {
                    errors.Add(error.ErrorMessage);
                }
            }
            string errorMessage = string.Join("\n", errors);
            return BadRequest(errorMessage);
        }
        return View(category);
    }
    public async Task<IActionResult> Delete(int Id)
    {
        CategoryModel category = await _dataContext.Categories.FindAsync(Id);
        _dataContext.Categories.Remove(category);
        await _dataContext.SaveChangesAsync();
        TempData["error"] = "sản phẩm đã xóa";
        return RedirectToAction("Index");
    }
    public async Task<IActionResult> Edit(int Id)
    {
        CategoryModel category = await _dataContext.Categories.FindAsync(Id);
        return View(category);
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(CategoryModel category)
    {
        if (ModelState.IsValid)
        {
            //code them du lieu
            category.Slug = category.Name.Replace(" ", "-");
            var slug = await _dataContext.Categories.FirstOrDefaultAsync(p => p.Slug == category.Slug && p.Id != category.Id);
            if (slug != null)
            {
                ModelState.AddModelError("", "San Pham da co trong database");
                return View(category);
            }
            _dataContext.Update(category);
            await _dataContext.SaveChangesAsync();
            TempData["success"] = "sửa sản phẩm thành công";
            return RedirectToAction("Index");
        }
        else
        {
            TempData["error"] = "Model có 1 vài thứ đang bị lỗi";
            List<string> errors = new List<string>();
            foreach (var value in ModelState.Values)
            {
                foreach (var error in value.Errors)
                {
                    errors.Add(error.ErrorMessage);
                }
            }
            string errorMessage = string.Join("\n", errors);
            return BadRequest(errorMessage);
        }
    }
}
