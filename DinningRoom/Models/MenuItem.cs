using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DinningRoom.Models
{
    public class MenuItem
    {
        public int Id { get; set; }
        public string NameEat { get; set; }
        public string NameCategory { get; set; }
        public int Price { get; set; }
    }
}
