using System.Diagnostics;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using KoiShowManagementSystem.Repositories.Entity;
using KoiShowManagementSystem.Services;

namespace KoiShowManagementSystem.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUserService _userService;
        public HomeController(IUserService userService)
        {
            _userService = userService;
        }

        // Trang chủ
        public IActionResult Index()
        {
            return View();
        }

        // GET: Account/Register
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        // POST: Account/Register
        [HttpPost]
        public async Task<IActionResult> Register(Users user)
        {
            if (ModelState.IsValid)
            {
                // Kiểm tra mật khẩu phải có ít nhất 6 ký tự
                if (user.Password.Length < 6)
                {
                    ModelState.AddModelError("Password", "Mật khẩu phải có ít nhất 6 ký tự.");
                    return View(user);
                }

                var result = await _userService.RegisterUserAsync(user);
                if (result)
                {
                    TempData["SuccessMessage"] = "Đăng ký thành công. Bạn có thể đăng nhập ngay bây giờ.";
                    return RedirectToAction("Login");
                }
                ModelState.AddModelError("", "Tên đăng nhập đã tồn tại.");
            }
            else
            {
                // Ghi lỗi vào cửa sổ Output của Visual Studio
                foreach (var state in ModelState)
                {
                    foreach (var error in state.Value.Errors)
                    {
                        Debug.WriteLine($"Lỗi ở {state.Key}: {error.ErrorMessage}");
                    }
                }
            }
            return View(user);
        }
        // GET: Account/Login
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // POST: Account/Login
        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            if (ModelState.IsValid)
            {
                var user = await _userService.AuthenticateUserAsync(username, password);
                if (user != null)
                {
                    // Tạo Claims
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                        new Claim(ClaimTypes.Name, user.Username),
                        new Claim(ClaimTypes.Role, user.Role)
                    };

                    var claimsIdentity = new ClaimsIdentity(claims, "UserAuth");
                    await HttpContext.SignInAsync("UserAuth", new ClaimsPrincipal(claimsIdentity));

                    // Thêm thông báo đăng nhập thành công
                    TempData["SuccessMessage"] = "Đăng nhập thành công!";
                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError("", "Tên đăng nhập hoặc mật khẩu không đúng.");
            }
            return View();
        }

        // GET: Account/Logout
        public async Task<IActionResult> Logout()
        {
            // Đăng xuất người dùng
            await HttpContext.SignOutAsync("UserAuth");

            // Thêm thông báo đăng xuất thành công
            TempData["SuccessMessage"] = "Đăng xuất thành công!";

            return RedirectToAction("Login");
        }
    }
}
