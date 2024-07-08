using DinningRoom.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace DinningRoom.Controllers
{
    [Authorize]
    public class AuthController : Controller
    {
        private readonly AppDbContext _context;

        public AuthController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(Employee employee)
        {
            if (ModelState.IsValid)
            {
                _context.Employees.Add(employee);
                await _context.SaveChangesAsync();
                return RedirectToAction("Login");
            }
            return View(employee);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            var user = await _context.Employees.FirstOrDefaultAsync(e => e.Email == email && e.Password == password);
            if (user != null)
            {
                // Выполните вход пользователя
                return RedirectToAction("Index", "Home");
            }
            ModelState.AddModelError("", "Неверный email или пароль.");
            return View();
        }

        public IActionResult Logout()
        {
            // Выполните выход пользователя
            return RedirectToAction("Index", "Home");
        }
    }
}
