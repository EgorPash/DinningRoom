using DinningRoom.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace DinningRoom.Controllers
{
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
                if (employee.Password != employee.ConfirmPassword)
                {
                    ModelState.AddModelError("ConfirmPassword", "Пароль и подтверждение пароля должны совпадать.");
                    return View(employee);
                }
                employee.Role = Request.Form["role"]; // Получение значения из формы
                _context.Employees.Add(employee);
                await _context.SaveChangesAsync();
                return RedirectToAction("ChooseMode", "Home");
            }
            return View(employee);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            var user = await _context.Employees.FirstOrDefaultAsync(e => e.Email == email && e.Password == password);
            if (user != null)
            {
                // Сохранение роли в Claims
                var role = await _context.Employees.Where(e => e.Email == email).Select(e => e.Role).FirstOrDefaultAsync();
                HttpContext.Session.SetString("UserRole", role);
                var claims = new List<Claim> 
                { 
                    new Claim(ClaimTypes.Name, email),
                    new Claim(ClaimTypes.Role, role)
                };
                // создаем объект ClaimsIdentity
                ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "Cookies");
                // установка аутентификационных куки
                await this.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError("", "Неверный email или пароль.");
            }
            return View();
        }

        public async Task<IActionResult> Logout()
        {
            await this.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            // Выполните выход пользователя
            return RedirectToAction("Index", "Home");
        }
    }
}
