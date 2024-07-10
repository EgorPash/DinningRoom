using DinningRoom.Models;
using DinningRoom.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static NuGet.Packaging.PackagingConstants;

namespace DinningRoom.Controllers
{
    [Authorize]
    public class MenuController : Controller
    {

        private readonly AppDbContext _dbContext;

        public MenuController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
            
        }


        public IActionResult Index()
        {
            var menus = _dbContext.MenuItems.ToList();
            return View(menus);
        }

        public IActionResult Order(int id)
        {
            ViewBag.Id = id;
            // var selectedItem = _menu.FirstOrDefault(item => item.Id == id);
            var orderItems = _dbContext.StringsOfOrders.Where(item => item.IdOrder == id).ToList();
            return View(orderItems);
            //if (selectedItem != null && s.Contains(selectedItem))
            //    {
            // _selectedItems.Add(selectedItem);
            //    }
        }

        [HttpPost]
        public IActionResult Order(List<int> selectedItems)
        {
            var menus = _dbContext.MenuItems.ToList();
            Orders orders = new Orders();

            orders.IdEmployee = 1;
            orders.DateOfOrder = DateTime.Now;
            orders.TotalSum = menus.Where(item => selectedItems.Contains(item.Id)).Sum(x => x.Price);
            _dbContext.Orders.Add(orders);
            _dbContext.SaveChanges();

            foreach (var item in selectedItems)
            {


                StringsOfOrder stringoforder = new StringsOfOrder();

                stringoforder.IdEmployee = 1;
                stringoforder.NameEat = menus.Where(X => X.Id == item).Select(x => x.NameEat).FirstOrDefault();
                stringoforder.IdEat = menus.Where(X => X.Id == item).Select(x => x.Id).FirstOrDefault();
                stringoforder.Quantity = 1;
                stringoforder.IdOrder = orders.Id;
                stringoforder.Price = menus.Where(X => X.Id == item).Select(x => x.Price).FirstOrDefault();
                _dbContext.StringsOfOrders.Add(stringoforder);
                _dbContext.SaveChanges();
            }

            return RedirectToAction("Order", new { id = orders.Id });

        }
        public IActionResult TableOfEatsToday()
        {
            var eat = new Dictionary<string, int>();
            var TodayOrders = _dbContext.Orders.Where(order => order.DateOfOrder.Date == DateTime.Now.Date).ToList();

            foreach (var OneOrder in TodayOrders)
            {
                var OneOrderItems = _dbContext.StringsOfOrders.Where(item => item.IdOrder == OneOrder.Id).ToList();

                foreach (var ItemsOfEats in OneOrderItems)
                {
                    // Содержит наименование блюда
                    string NameEatOfOrder = ItemsOfEats.NameEat;

                    if (!eat.ContainsKey(NameEatOfOrder))
                    {
                        eat[NameEatOfOrder] = 1;
                    }
                    else
                    {
                        eat[NameEatOfOrder] += 1;
                    }
                }
            }
            return View(eat);
        }
    }
}