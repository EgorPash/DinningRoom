using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DinningRoom.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using DinningRoom.Models;
using Microsoft.EntityFrameworkCore;

namespace DinningRoom.Controllers
{
    public class MenuController : Controller
    {
        public readonly List<MenuItemModel> _menu;

        private readonly AppDbContext _dbContext;

        public MenuController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public MenuController()
        {
            _menu = new List<MenuItemModel>
            {
                new MenuItemModel { Id = 1, Name = "Бургер", Description = "Вкусный бургер", Price = 10.99m },
                new MenuItemModel { Id = 2, Name = "Салат", Description = "Свежий салат", Price = 7.50m },
                new MenuItemModel { Id = 3, Name = "Пицца", Description = "Ароматная пицца", Price = 15.75m }
            };
        }

        public IActionResult Index()
        {
            return View(_menu);
        }

        public IActionResult Order(int id)
        {
            var selectedItem = _menu.FirstOrDefault(item => item.Id == id);
            if (selectedItem != null && !MyStaticClass._selectedItems.Contains(selectedItem))
            {
                // _selectedItems.Add(selectedItem);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Order(List<int> selectedItems)
        {
            MyStaticClass._selectedItems = _menu.Where(item => selectedItems.Contains(item.Id)).ToList();
            return RedirectToAction("TableOfOrders");
        }

        public IActionResult TableOfOrders()
        {
            return View(MyStaticClass._selectedItems);
        }

        [HttpPost]
        public IActionResult AddMenuItem(MenuItemModel newItem)
        {
            if (ModelState.IsValid)
            {
                _dbContext.Menus.Add(new Menu { NameEat = newItem.Name, Price = newItem.Price, IdCategory = newItem.CategoryId });
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
                var existingItem = dbContext.Menus.Find(updatedItem.Id);
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