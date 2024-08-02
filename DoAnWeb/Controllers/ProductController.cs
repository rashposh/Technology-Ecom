using DoAnWeb.Models.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DoAnWeb.Controllers
{
    public class ProductController : Controller
    {
        private readonly DataContext _dataContext;
        public ProductController(DataContext context)
        {
            _dataContext = context;

		}
        public IActionResult Index(string search)
        {
            if (string.IsNullOrEmpty(search))
            {
                return LocalRedirect("/");
            }
            ViewBag.Search = search;
			var products = _dataContext.Products.Where(p => p.Name.ToLower().Contains(search.ToLower()))
                .Include(p=>p.Brad)
                .Include(p => p.Category)
                .ToList();
			return View(products);
        }
        public async Task<IActionResult> Details(int Id)
        {
            if (Id == null)
            {
                return RedirectToAction("Index");
            }


			var productByCategory = _dataContext.Products.Where(p => p.Id == Id)
                .Include(p => p.Category).FirstOrDefault();

            var recommended = _dataContext.Products.Include(p => p.Category)
                .Where(p => p.Category == productByCategory.Category).ToList();
            ViewBag.Recommended = recommended;

            return View(productByCategory);
        }
    }
}
