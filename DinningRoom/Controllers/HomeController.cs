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

namespace DinningRoom.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _dbContext;

        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, AppDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        public IActionResult Index()
        {
            return View();
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
