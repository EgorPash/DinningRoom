using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DinningRoom.ViewModels
{
    public class MenuItemModel
    {
        public int Id { get; set; }
        public string NameEat { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
        public int Order { get; set; }
        public int IdCategory { get; set; }
        public string NameCategory { get; set; }
    }
}
