using DinningRoom.Models;
using DinningRoom.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static NuGet.Packaging.PackagingConstants;

namespace DinningRoom.Controllers
{
    public class MenuController : Controller
    {
        public readonly List<MenuItemModel> _menu;

        private readonly AppDbContext _dbContext;

        public MenuController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
            _menu = new List<MenuItemModel>
            {
                new MenuItemModel { Id = 1, NameEat = "Бургер", Description = "Вкусный бургер", Price = 11 },
                new MenuItemModel { Id = 2, NameEat = "Салат", Description = "Свежий салат", Price = 8 },
                new MenuItemModel { Id = 3, NameEat = "Пицца", Description = "Ароматная пицца", Price = 16 }
            };
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

            Orders orders = new Orders();

            orders.IdEmployee = 1;
            orders.DateOfOrder = DateTime.Now;
            orders.TotalSum = _menu.Where(item => selectedItems.Contains(item.Id)).Sum(x => x.Price);
            _dbContext.Orders.Add(orders);
            _dbContext.SaveChanges();

            foreach (var item in selectedItems)
            {


                StringsOfOrder stringoforder = new StringsOfOrder();

                stringoforder.IdEmployee = 1;
                stringoforder.NameEat = _menu.Where(X => X.Id == item).Select(x => x.NameEat).FirstOrDefault();
                stringoforder.IdEat = _menu.Where(X => X.Id == item).Select(x => x.Id).FirstOrDefault();
                stringoforder.Quantity = 1;
                stringoforder.IdOrder = orders.Id;
                stringoforder.Price = _menu.Where(X => X.Id == item).Select(x => x.Price).FirstOrDefault();
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