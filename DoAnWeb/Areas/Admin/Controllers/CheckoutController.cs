using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DoAnWeb.Models;
using System.Linq;
using DoAnWeb.Models.Repository;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;

namespace DoAnWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]

    public class CheckoutController : Controller
    {
        private readonly DataContext _context;

        public CheckoutController(DataContext context)
        {
            _context = context;
        }

        // GET: Admin/Checkout
        public IActionResult Index()
        {
            var pendingOrders = _context.Orders
                                        .Include(o => o.OrderDetails)
                                        .ThenInclude(od => od.Product)
                                        .OrderByDescending(o => o.CreatedDate)
                                        .ToList();

            return View(pendingOrders);
        }

        [HttpPost]
        public IActionResult UpdateOrderStatus(int id, int status)
        {
            var order = _context.Orders.Find(id);
            if (order != null)
            {
                order.Status = status;
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}
