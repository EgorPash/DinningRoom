using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DinningRoom.Models
{
    public class Orders
    {
        public int Id { get; set; }
        public int IdEmployee { get; set; }
        public int DateOfOrder { get; set; }
        public int TotalSum { get; set; }
    }
}
