using DoAnWeb.Models;
using DoAnWeb.Models.Repository;
using DoAnWeb.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.InteropServices;
using System.Security.Claims;

namespace DoAnWeb.Controllers
{
    public class AccountController : Controller
    {
        private UserManager<IdentityUser> _userManage;
        private SignInManager<IdentityUser> _signInManager;
        private readonly DataContext _dataContext;
        private readonly RoleManager<IdentityRole> _roleManager;
        public AccountController(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManage,
             DataContext dataContext, RoleManager<IdentityRole> roleManager)
        {
            _signInManager = signInManager;
            _userManage = userManage;
            _dataContext = dataContext;
            _roleManager = roleManager;
        }

        public IActionResult Login(string returnUrl)
        {
            return View(new LoginViewModel { });
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginVM, string ReturnUrl = "/")
        {
            if (!ModelState.IsValid)
            {
                return View(loginVM);
            }
            Microsoft.AspNetCore.Identity.SignInResult result = await _signInManager.PasswordSignInAsync(loginVM.Username, loginVM.Password, false, false);

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Invalid Username and Password");
                return View(loginVM);
            }
            var user = await _userManage.FindByNameAsync(loginVM.Username);

            if (await _userManage.IsInRoleAsync(user,"Admin"))
            {
                return Redirect("/Admin");
            }
            return LocalRedirect(ReturnUrl);

        }
        public async Task<IActionResult> Create()
        {
            return View();
        }

        public IActionResult AccessDenied()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(UserModel user)
        {
            if (ModelState.IsValid)
            {
                AppUserModel newUser = new AppUserModel { UserName = user.Username, Email = user.Email };
                IdentityResult result = await _userManage.CreateAsync(newUser, user.Password);
                if (result.Succeeded)
                {
                    TempData["success"] = "tạo tài khoản thành công ";
                    return Redirect("/account/login");
                }
                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(user);
        }
        public async Task<IActionResult> Logout(string returnUrl = "/")
        {
            await _signInManager.SignOutAsync();
            return Redirect(returnUrl);
        }
    }
}