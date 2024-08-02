using Microsoft.AspNetCore.Mvc;

namespace DoAnWeb.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
