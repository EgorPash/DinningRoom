using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using DinningRoom.Models;
using DinningRoom.ViewModels;
using Microsoft.EntityFrameworkCore;
using static NuGet.Packaging.PackagingConstants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace DinningRoom.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly AppDbContext _dbContext;

        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, AppDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        [HttpGet]
        public IActionResult ChooseMode()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ChooseMode(string mode)
        {
            // Сохранение выбранного режима в сессии
            HttpContext.Session.SetString("SelectedMode", mode);

            return RedirectToAction("Index");
        }

        public IActionResult Index()
        {
            // Получение выбранного режима из сессии
            string selectedMode = HttpContext.Session.GetString("SelectedMode");

            // Проверка роли пользователя
            string userRole = User.FindFirst(ClaimTypes.Role)?.Value;

            // Настройка отображения меню в зависимости от роли и выбранного режима
            if (selectedMode == "Сотрудник компании" && userRole == "Сотрудник компании")
            {
                return View("IndexForCompanyEmployee");
            }
            else if (selectedMode == "Сотрудник столовой" && userRole == "Сотрудник столовой")
            {
                return View("IndexForDiningEmployee");
            }
            else if (userRole == "Администратор")
            {
                return View(); // Используйте стандартный Index для администратора
            }
            else
            {
                return RedirectToAction("ChooseMode"); // Перенаправление на выбор режима, если пользователь не авторизован
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult EditMenu()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new Models.ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult NewOrder ()
        {
            return View();
        }

        public IActionResult TableOfOrders()
        {
            var orders = _dbContext.Orders.OrderByDescending(o => o.Id).ToList();

            return View(orders);
        }
        public IActionResult Delete(int id)
        {
            var IdOrder = _dbContext.Orders.Find(id);
            var orderItems = _dbContext.StringsOfOrders.Where(item => item.IdOrder == id).ToList();
            foreach (var NameEat in orderItems)
            {
                _dbContext.StringsOfOrders.Remove(NameEat);  
            }
            if (IdOrder != null)
            {
                _dbContext.Orders.Remove(IdOrder);
            }
            _dbContext.SaveChanges();
            return RedirectToAction("TableOfOrders"); // или другой метод для перенаправления
        }
    }
}
