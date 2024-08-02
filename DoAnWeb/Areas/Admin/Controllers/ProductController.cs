using DoAnWeb.Models;
using DoAnWeb.Models.Repository;
using DoAnWeb.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace DoAnWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ProductController : Controller
    {
        private readonly DataContext _dataContext;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductController(DataContext context, IWebHostEnvironment webHostEnvironment)
        {
            _dataContext = context;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _dataContext.Products.OrderByDescending(p => p.Id).Include(p => p.Category).Include(p => p.Brad).ToListAsync());
        }

        public IActionResult Create()
        {
            ViewBag.Categories = new SelectList(_dataContext.Categories, "Id", "Name");
            ViewBag.Brands = new SelectList(_dataContext.Brands, "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductViewModel product)
        {
            ViewBag.Categories = new SelectList(_dataContext.Categories, "Id", "Name", product.Category);
            ViewBag.Brands = new SelectList(_dataContext.Brands, "Id", "Name", product.Brad);




            if (ModelState.IsValid)
            {
                var productModel = new ProductModel();
                productModel.Brad = await _dataContext.Brands.FirstAsync(b => b.Id == product.Brad);
                productModel.Category = await _dataContext.Categories.FirstAsync(b => b.Id == product.Category);
                productModel.Price = product.Price;
                productModel.Description = product.Description;
                productModel.Name = product.Name;


                productModel.Slug = product.Name.Replace(" ", "-");
                var slug = await _dataContext.Products.FirstOrDefaultAsync(p => p.Slug == productModel.Slug);
                if (slug != null)
                {
                    ModelState.AddModelError("", "San Pham da co trong database");
                    return View(product);
                }
                if (product.ImageUpload != null)
                {
                    string uploadsDir = Path.Combine(_webHostEnvironment.WebRootPath, "media/Products");
                    string imageName = Guid.NewGuid().ToString() + "_" + product.ImageUpload.FileName;
                    string filePath = Path.Combine(uploadsDir, imageName);

                    using (FileStream fs = new FileStream(filePath, FileMode.Create))
                    {
                        await product.ImageUpload.CopyToAsync(fs);
                    }
                    productModel.Image = imageName;
                }

                _dataContext.Add(productModel);
                await _dataContext.SaveChangesAsync();
                TempData["success"] = "Thêm sản phẩm thành công";
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
            return View(product);
        }

        [Route("Edit")]
        public async Task<IActionResult> Edit(int Id)
        {
            ProductModel product = await _dataContext.Products.Include(p => p.Category).Include(p => p.Brad).FirstAsync(p => p.Id == Id);
            if (product == null)
            {
                return NotFound();
            }
            ViewBag.Categories = new SelectList(_dataContext.Categories, "Id", "Name", product.Category?.Id);
            ViewBag.Brands = new SelectList(_dataContext.Brands, "Id", "Name", product.Brad?.Id);
            var pVM = new ProductViewModel()
            {
                Id = Id,
                Name = product.Name,
                Brad = product.Brad.Id,
                Category = product.Category.Id,
                Description = product.Description,
                Image = product.Image,
                ImageUpload = product.ImageUpload,
                Price = product.Price,
            };

            return View(pVM);
        }

        [Route("Edit")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int Id, ProductViewModel product)
        {
            ViewBag.Categories = new SelectList(_dataContext.Categories, "Id", "Name", product.Category);
            ViewBag.Brands = new SelectList(_dataContext.Brands, "Id", "Name", product.Brad);
            if (ModelState.IsValid)
            {
                var productModel = await _dataContext.Products.Include(p => p.Category).Include(p => p.Brad).FirstAsync(x => x.Id == product.Id);
                productModel.Slug = product.Name.Replace(" ", "-");

                if (product.ImageUpload != null)
                {
                    string uploadsDir = Path.Combine(_webHostEnvironment.WebRootPath, "media/Products");
                    string imageName = Guid.NewGuid().ToString() + "_" + product.ImageUpload.FileName;
                    string filePath = Path.Combine(uploadsDir, imageName);

                    using (FileStream fs = new FileStream(filePath, FileMode.Create))
                    {
                        await product.ImageUpload.CopyToAsync(fs);
                    }
                    productModel.Image = imageName;

                    string oldfileImage = Path.Combine(uploadsDir, productModel.Image);
                    if (System.IO.File.Exists(oldfileImage))
                    {
                        System.IO.File.Delete(oldfileImage);
                    }
                }

                productModel.Name = product.Name;
                productModel.Description = product.Description;
                productModel.Price = product.Price;

                var cat = await _dataContext.Categories.FirstOrDefaultAsync(x => x.Id == product.Category);
                var brad = await _dataContext.Brands.FirstOrDefaultAsync(x => x.Id == product.Brad);

                productModel.Category = cat;
                productModel.Brad = brad;

                _dataContext.Update(productModel);
                await _dataContext.SaveChangesAsync();
                TempData["success"] = "Cập nhật sản phẩm thành công";
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

        public async Task<IActionResult> Delete(int Id)
        {
            ProductModel product = await _dataContext.Products.FindAsync(Id);
            if (product == null)
            {
                return NotFound();
            }
            string uploadsDir = Path.Combine(_webHostEnvironment.WebRootPath, "media/Products");
            string oldfileImage = Path.Combine(uploadsDir, product.Image);
            if (System.IO.File.Exists(oldfileImage))
            {
                System.IO.File.Delete(oldfileImage);
            }
            _dataContext.Products.Remove(product);
            await _dataContext.SaveChangesAsync();
            TempData["error"] = "Sản phẩm đã xóa";
            return RedirectToAction("Index");
        }
    }
}
