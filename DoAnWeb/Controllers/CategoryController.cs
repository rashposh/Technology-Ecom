using DoAnWeb.Models;
using DoAnWeb.Models.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DoAnWeb.Controllers
{
    public class CategoryController : Controller
    {
        private readonly DataContext _dataContext;
        public CategoryController(DataContext context)
        {
            _dataContext = context;
        }
        public async Task<IActionResult> Index(string Slug = "")
        {
            CategoryModel category = _dataContext.Categories.Where(c => c.Slug == Slug).FirstOrDefault();
            if (category == null) { return RedirectToAction("Index"); }
            var productByCategory = _dataContext.Products.Where(p => p.Category.Id == category.Id);
            // FirstOrDefault ra 1 gia tri, ma orderby ra 1 mang :v, di nhien nos wtf r ok :v
            return View(await productByCategory.OrderByDescending(p => p.Id).ToListAsync());
        }
    }
}
