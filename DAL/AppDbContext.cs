using Shalev_Elah_HW6.Models;
using System.Data;
using Microsoft.EntityFrameworkCore;

namespace Shalev_Elah_HW6.DAL
{
    public class AppDbContext : DbContext
    {
        //constructor
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        //create db set
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders{ get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<Supplier> Supplierss { get; set; }
        public DbSet<SupplierDetail> SupplierDetails { get; set; }

    }
}
