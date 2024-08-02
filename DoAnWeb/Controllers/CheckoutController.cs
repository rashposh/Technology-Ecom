using DoAnWeb.Models;
using DoAnWeb.Models.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System;
using System.Threading.Tasks;
using DoAnWeb.Models.Repository.Compoments;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace DoAnWeb.Controllers;

[Authorize]
public class CheckoutController : Controller
{
    private readonly DataContext _dataContext;

    public CheckoutController(DataContext context)
    {
        _dataContext = context;
    }

    public IActionResult Index()
    {
        var userEmail = User.FindFirstValue(ClaimTypes.Email);

        var orders = _dataContext.Orders.Where(o => o.UserName == userEmail)
            .Include(o => o.OrderDetails)
            .Include("OrderDetails.Product")
            .OrderByDescending(o => o.CreatedDate)
            .ToList();

        return View(orders);
    }

    public async Task<IActionResult> Checkout()
    {
        List<CartItemModel> cartItems = HttpContext.Session.GetJson<List<CartItemModel>>("Cart") ?? new List<CartItemModel>();

        var userEmail = User.FindFirstValue(ClaimTypes.Email);
        if (userEmail == null)
        {
            return RedirectToAction("Login", "Account");
        }

        var ordercode = Guid.NewGuid().ToString();

        var orderItem = new OrderModel
        {
            OrderCode = ordercode,
            UserName = userEmail,
            Status = 1,
            CreatedDate = DateTime.Now,
            
        };

        List<OrderDetails> orderDetails = cartItems.Select(i =>
        {
            var product = _dataContext.Products.Find(i.ProductId);

            var od = new OrderDetails() { };
            od.Product = product;
            od.Order = orderItem;
            od.Price = i.Price;
            od.Quanlity = i.Quantity;
            return od;
        }).ToList();


        orderItem.OrderDetails = orderDetails;

        _dataContext.Add(orderItem);
        await _dataContext.SaveChangesAsync();
        
        HttpContext.Session.SetJson("Cart", new List<object>());

        TempData["success"] = "Đơn hàng đã được tạo";
            
        return RedirectToAction("Index");
    }
}
