using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DinningRoom.Models
{
    public class Menu
    {
        public int Id { get; set; }
        public string NameEat { get; set; }
        public int Price { get; set; }
        public int IdCategory { get; set; }
    }
}
