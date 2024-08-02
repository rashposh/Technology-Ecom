using DoAnWeb.Models;
using DoAnWeb.Models.Repository;
using DoAnWeb.Models.Repository.Compoments;
using DoAnWeb.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DoAnWeb.Controllers;

[Authorize]
public class CartController : Controller
{
    private readonly DataContext _dataContext;

    public CartController(DataContext _context)
    {
        _dataContext = _context;
    }
    public IActionResult Index()
    {
        List<CartItemModel> cartItems = HttpContext.Session.GetJson<List<CartItemModel>>("Cart") ?? new List<CartItemModel>();
        CartItemViewModel CartVM = new()
        {
            CartItems = cartItems,
            GrandTotal = cartItems.Sum(x => x.Quantity * x.Price)
        };
        return View(CartVM);
    }
    public IActionResult Checkout()
    {
        return LocalRedirect("/Checkout");
    }
    public async Task<IActionResult> Add(int Id, int quantity = 1)
    {
        ProductModel product = await _dataContext.Products.FindAsync(Id);
        List<CartItemModel> cart = HttpContext.Session.GetJson<List<CartItemModel>>("Cart") ?? new List<CartItemModel>();
        CartItemModel cartItems = cart.Where(c => c.ProductId == Id).FirstOrDefault();

        if (cartItems == null)
        {
            var cartItem = new CartItemModel(product);
            cartItem.Quantity = quantity;
			cart.Add(cartItem);
        }
        else
        {
            cartItems.Quantity += quantity;
        }
        HttpContext.Session.SetJson("Cart", cart);

        TempData["success"] = "Add Item to cart Successfully";
        return Redirect(Request.Headers["Referer"].ToString());
    }
    public async Task<IActionResult> Decrease(int Id)
    {
        List<CartItemModel> cart = HttpContext.Session.GetJson<List<CartItemModel>>("Cart");
        CartItemModel cartItem = cart.Where(c => c.ProductId == Id).FirstOrDefault();
        if (cartItem.Quantity > 1)
        {
            --cartItem.Quantity;
        }
        else
        {
            cart.RemoveAll(p => p.ProductId == Id);
        }
        if (cart.Count == 0)
        {
            HttpContext.Session.Remove("Cart");
        }
        else
        {
            HttpContext.Session.SetJson("Cart", cart);
        }
        TempData["success"] = "Decrease Item quantity to cart Successfully";
        return RedirectToAction("Index");
    }
    public async Task<IActionResult> Increase(int Id)
    {
        List<CartItemModel> cart = HttpContext.Session.GetJson<List<CartItemModel>>("Cart");
        CartItemModel cartItem = cart.Where(c => c.ProductId == Id).FirstOrDefault();
        if (cartItem.Quantity >= 1)
        {
            ++cartItem.Quantity;
        }
        else
        {
            cart.RemoveAll(p => p.ProductId == Id);
        }
        if (cart.Count == 0)
        {
            HttpContext.Session.Remove("Cart");
        }
        else
        {
            HttpContext.Session.SetJson("Cart", cart);
        }
        TempData["success"] = "Increase Item quantity to cart Successfully";
        return RedirectToAction("Index");
    }
    public async Task<IActionResult> Remove(int Id)
    {
        List<CartItemModel> cart = HttpContext.Session.GetJson<List<CartItemModel>>("Cart");
        cart.RemoveAll(p => p.ProductId == Id);
        if (cart.Count == 0)
        {
            HttpContext.Session.Remove("Cart");
        }
        else
        {
            HttpContext.Session.SetJson("Cart", cart);
        }
        TempData["success"] = "Remove Item of cart Successfully";
        return RedirectToAction("Index");
    }
    public async Task<IActionResult> Clear()
    {
        HttpContext.Session.Remove("Cart");
        TempData["success"] = "Clear Item of cart Successfully";
        return RedirectToAction("Index");
    }
}
