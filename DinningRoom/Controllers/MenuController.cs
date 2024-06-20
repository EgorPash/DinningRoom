using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DinningRoom.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace DinningRoom.Controllers
{
    public class MenuController : Controller
    {
        public readonly List<MenuItemModel> _menu;
        

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
    }
}