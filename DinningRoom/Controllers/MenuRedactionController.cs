using DinningRoom.Models;
using DinningRoom.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace DinningRoom.Controllers
{
    public class MenuRedactionController : Controller
    {
        public readonly List<MenuItemModel> _menuItems;

        private readonly AppDbContext _context;
        public MenuRedactionController(AppDbContext context)
        {
            _context = context;
            //_menuItems = new List<MenuItemModel>
            //{
            //    new MenuItemModel { Id = 1, Name = "Бургер", Description = "Вкусный бургер", Price = 11 },
            //    new MenuItemModel { Id = 2, Name = "Салат", Description = "Свежий салат", Price = 8 },
            //    new MenuItemModel { Id = 3, Name = "Пицца", Description = "Ароматная пицца", Price = 16 }
            //};
        }
        public IActionResult Index()
        {
            var EatsItems = _context.MenuItems.ToList();
            return View(EatsItems);
        }

        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        public IActionResult Create(MenuItemModel newItem)
        {
            if (ModelState.IsValid)
            {
                _context.MenuItems.Add(new MenuItem { NameEat = newItem.NameEat, Price = newItem.Price, NameCategory = newItem.NameCategory });
                _context.SaveChanges();
                return RedirectToAction("Index"); // или другой метод для перенаправления
            }
            return View(newItem);
        }
        public IActionResult Delete(int id)
        {
            var menuItem = _context.MenuItems.Find(id);
            if (menuItem != null)
            {
                _context.MenuItems.Remove(menuItem);
                _context.SaveChanges();
            }
            return RedirectToAction("Index"); // или другой метод для перенаправления
        }

        public IActionResult Edit(int id)
        {
            ViewBag.Id = id;
            var EatItem = _context.MenuItems.Where(item => item.Id == id).ToList()[0];
            return View(EatItem);
        }


        [HttpPost]
        public IActionResult Edit(MenuItemModel updatedItem)
        {
            if (ModelState.IsValid)
            {
                var existingItem = _context.MenuItems.Find(updatedItem.Id);
                if (existingItem != null)
                {
               
                    existingItem.NameEat = updatedItem.NameEat;
                    existingItem.Price = updatedItem.Price;
                    existingItem.NameCategory = updatedItem.NameCategory;
                    _context.SaveChanges();
                    return RedirectToAction("Index"); // или другой метод для перенаправления
                }
            }
            return View(updatedItem);
        }
    }
}
