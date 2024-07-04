using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DinningRoom.Models
{
    public class Eats
    {
        public int IdEat { get; set; }
        public string NameEat { get; set; }
        public string NameCategory { get; set; }
        public int Quantity { get; set; }
        public int IdOrder { get; set; }

    }
}
