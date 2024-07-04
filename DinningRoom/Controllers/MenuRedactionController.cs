using DinningRoom.Models;
using DinningRoom.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace DinningRoom.Controllers
{
    public class MenuRedactionController : Controller
    {
        public readonly List<MenuItemModel> _menuItems;

        private readonly AppDbContext _context;
        public MenuRedactionController(AppDbContext context)
        {
            _context = context;
            _menuItems = new List<MenuItemModel>
            {
                new MenuItemModel { Id = 1, Name = "Бургер", Description = "Вкусный бургер", Price = 11 },
                new MenuItemModel { Id = 2, Name = "Салат", Description = "Свежий салат", Price = 8 },
                new MenuItemModel { Id = 3, Name = "Пицца", Description = "Ароматная пицца", Price = 16 }
            };
        }
        public IActionResult Index()
        {
            return View(_menuItems);
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
                _context.Menus.Add(new Menu { NameEat = newItem.Name, Price = newItem.Price, IdCategory = newItem.IdCategory });
                _context.SaveChanges();
                return RedirectToAction("Index"); // или другой метод для перенаправления
            }
            return View(newItem);
        }
        public IActionResult DeleteMenuItem(int id)
        {
            var menuItem = _context.Menus.Find(id);
            if (menuItem != null)
            {
                _context.Menus.Remove(menuItem);
                _context.SaveChanges();
            }
            return RedirectToAction("Index"); // или другой метод для перенаправления
        }

        [HttpPost]
        public IActionResult UpdateMenuItem(MenuItemModel updatedItem)
        {
            if (ModelState.IsValid)
            {
                var existingItem = _context.Menus.Find(updatedItem.Id);
                if (existingItem != null)
                {
                    existingItem.NameEat = updatedItem.Name;
                    existingItem.Price = updatedItem.Price;
                    existingItem.IdCategory = updatedItem.IdCategory;
                    _context.SaveChanges();
                    return RedirectToAction("Index"); // или другой метод для перенаправления
                }
            }
            return View(updatedItem);
        }
    }
}
