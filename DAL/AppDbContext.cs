using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Group14_BevoBooks.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Group14_BevoBooks.DAL
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        //You need one db set for each model class. 
        public DbSet<Book> Books { get; set; }
        public DbSet<BookOrder> BookOrders { get; set; }
        public DbSet<CreditCard> CreditCards { get; set; }
        public DbSet<Discount> Discounts { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Shipping> ShippingPrices { get; set; }
        //public DbSet<BookOrderDetail> BookOrderDetails { get; set; }
        public DbSet<DefaultReorder> ReorderQuantity { get; set; }
    }
}