using DoAnWeb.Models;
using DoAnWeb.Models.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DoAnWeb.Controllers
{

    public class BradController : Controller
    {
        private readonly DataContext _dataContext;
        public BradController(DataContext context)
        {
            _dataContext = context;
        }
        public async Task<IActionResult> Index(string Slug = "")
        {
            BradModel bard = _dataContext.Brands.Where(c => c.Slug == Slug).FirstOrDefault();
            if (bard == null) { return RedirectToAction("Index"); }
            var productByBard = _dataContext.Products.Where(p => p.Brad.Id == bard.Id);
            // FirstOrDefault ra 1 gia tri, ma orderby ra 1 mang :v, di nhien nos wtf r ok :v
            return View(await productByBard.OrderByDescending(p => p.Id).ToListAsync());
        }
    }
}
