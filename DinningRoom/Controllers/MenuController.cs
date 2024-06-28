using DinningRoom.Models;
using DinningRoom.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
                new MenuItemModel { Id = 1, Name = "Бургер", Description = "Вкусный бургер", Price = 11 },
                new MenuItemModel { Id = 2, Name = "Салат", Description = "Свежий салат", Price = 8 },
                new MenuItemModel { Id = 3, Name = "Пицца", Description = "Ароматная пицца", Price = 16 }
            };
        }


        public IActionResult Index()
        {
            return View(_menu);
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
                stringoforder.NameEat = _menu.Where(X => X.Id == item).Select(x => x.Name).FirstOrDefault();
                stringoforder.IdEat = _menu.Where(X => X.Id == item).Select(x => x.Id).FirstOrDefault();
                stringoforder.Quantity = 1;
                stringoforder.IdOrder = orders.Id;
                stringoforder.Price = _menu.Where(X => X.Id == item).Select(x => x.Price).FirstOrDefault();
                _dbContext.StringsOfOrders.Add(stringoforder);
                _dbContext.SaveChanges();
            }

            return RedirectToAction("Order", new { id = orders.Id });


        }


        public IActionResult TableOfOrders()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddMenuItem(MenuItemModel newItem)
        {
            if (ModelState.IsValid)
            {
                _dbContext.Menus.Add(new Menu { NameEat = newItem.Name, Price = newItem.Price, IdCategory = newItem.IdCategory });
                _dbContext.SaveChanges();
                return RedirectToAction("Index"); // или другой метод для перенаправления
            }
            return View(newItem);
        }
        public IActionResult DeleteMenuItem(int id)
        {
            var menuItem = _dbContext.Menus.Find(id);
            if (menuItem != null)
            {
                _dbContext.Menus.Remove(menuItem);
                _dbContext.SaveChanges();
            }
            return RedirectToAction("Index"); // или другой метод для перенаправления
        }

        [HttpPost]
        public IActionResult UpdateMenuItem(MenuItemModel updatedItem)
        {
            if (ModelState.IsValid)
            {
                var existingItem = _dbContext.Menus.Find(updatedItem.Id);
                if (existingItem != null)
                {
                    existingItem.NameEat = updatedItem.Name;
                    existingItem.Price = updatedItem.Price;
                    existingItem.IdCategory = updatedItem.IdCategory;
                    _dbContext.SaveChanges();
                    return RedirectToAction("Index"); // или другой метод для перенаправления
                }
            }
            return View(updatedItem);
        }
    }
}