using DoAnWeb.Models;
using DoAnWeb.Models.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DoAnWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]

    public class BradController : Controller
    {
        private readonly DataContext _dataContext;

        public BradController(DataContext context)
        {
            _dataContext = context;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _dataContext.Brands.OrderByDescending(p => p.Id).ToListAsync());
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BradModel brad)
        {
            if (ModelState.IsValid)
            {
                //code them du lieu
                brad.Slug = brad.Name.Replace(" ", "-");
                var slug = await _dataContext.Brands.FirstOrDefaultAsync(p => p.Slug == brad.Slug);
                if (slug != null)
                {
                    ModelState.AddModelError("", "thương hiệu da co trong database");
                    return View(brad);
                }
                _dataContext.Add(brad);
                await _dataContext.SaveChangesAsync();
                TempData["success"] = "Thêm thương hiệu thành công";
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
            return View(brad);
        }
        public async Task<IActionResult> Delete(int Id)
        {
            BradModel brad = await _dataContext.Brands.FindAsync(Id);
            _dataContext.Brands.Remove(brad);
            await _dataContext.SaveChangesAsync();
            TempData["error"] = "sản phẩm đã xóa";
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Edit(int Id)
        {
            var brad = await _dataContext.Brands.FindAsync(Id);
            return View(brad);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(BradModel brad)
        {
            if (ModelState.IsValid)
            {
                //code them du lieu
                brad.Slug = brad.Name.Replace(" ", "-");
                var slug = await _dataContext.Categories.FirstOrDefaultAsync(p => p.Slug == brad.Slug && p.Id != brad.Id);
                if (slug != null)
                {
                    ModelState.AddModelError("", "San Pham da co trong database");
                    return View(brad);
                }
                _dataContext.Update(brad);
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
}
