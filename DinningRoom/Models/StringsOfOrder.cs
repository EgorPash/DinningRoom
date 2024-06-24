using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DinningRoom.Models
{
    public class StringsOfOrder
    {
        public int Id { get; set; }
        public int IdEmployee { get; set; }
        public string NameEat { get; set; }
        public int IdEat { get; set; }
        public int Quantity { get; set; }
        public int IdOrder { get; set; }
    }
}
