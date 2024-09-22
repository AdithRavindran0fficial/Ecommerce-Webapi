using BCrypt.Net;
using Ecommerce_Webapi.Models;
using Ecommerce_Webapi.Models.UserModel;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce_Webapi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions option)
            :base(option)
        {

        }
        public DbSet<Users> Users { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Order>Orders { get; set; }
        public DbSet<OrderItems> OrderItems { get; set; }
        public DbSet<Products> Products { get; set; }
        public DbSet<WhishList> WhishList { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Users>().HasData(new Users {Id=1, UserName="Adith",UserEmail="Adith1@gmial.com",Password=BCrypt.Net.BCrypt.EnhancedHashPassword("password"),Role="Admin",IsStatus=true });
        }
    }
}
