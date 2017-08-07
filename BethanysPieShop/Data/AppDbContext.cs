using BethanysPieShop.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BethanysPieShop.Data
{
    //public class AppDbContext : DbContext
    // to add support to Identity, extend the IdentityDbContext instead (or create another one)
    // AppDbContext is used to map database to code (1 database <=> 1 DbContext)
    // TODO somehow it doesnt work with MySQL
    public class AppDbContext : IdentityDbContext<IdentityUser>
    {
        // either call super constructor with DbContextOptions or override OnConfiguring method to configure the Database context
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        //Each DbSet property corresponds to a database table and are access points to these
        // Categories table
        public DbSet<Category> Categories { get; set; }
        // Pies table
        public DbSet<Pie> Pies { get; set; }
        public DbSet<ShoppingCartItem> ShoppingCartItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
    }
}
