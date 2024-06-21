using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace DinningRoom.Models
{
    public class AppDbContext : DbContext
    {
        public DbSet<Menu> Menus { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<StringsOfOrder> StringsOfOrders { get; set; }
        public DbSet<Orders> Orders { get; set; }
        public DbSet<Employee> Employees { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
    }
}
