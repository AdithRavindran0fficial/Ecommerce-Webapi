using BCrypt.Net;
using Ecommerce_Webapi.DTOs.WhishListDTO;
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
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItems> OrderItems { get; set; }
        public DbSet<Products> Products { get; set; }
        public DbSet<WhishList> WhishList { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var salt = BCrypt.Net.BCrypt.GenerateSalt();
            modelBuilder.Entity<Users>().HasData(new Users { Id = 9, UserName = "Admin", UserEmail = "Admin.com", Password = BCrypt.Net.BCrypt.HashPassword("password",salt), Role = "Admin", IsStatus = true });

            modelBuilder.Entity<Products>()
                .Property(pr => pr.Price).
                HasPrecision(18, 2);

            modelBuilder.Entity<Products>()
                .HasOne(pr => pr.Category)
                .WithMany(pr => pr.Products)
                .HasForeignKey(pr => pr.CategoryId);

            modelBuilder.Entity<Cart>()
                .HasOne(us => us.Users)
                .WithOne(ct => ct.Cart)
                .HasForeignKey<Cart>(us => us.UserId);

            modelBuilder.Entity<CartItem>()
                .HasOne(ct => ct.Cart)
                .WithMany(cti => cti.CartItems)
                .HasForeignKey(ct => ct.CartId);

            modelBuilder.Entity<CartItem>()
                .HasOne(cr => cr.Products)
                .WithMany(pr => pr.CartItems)
                .HasForeignKey(cr => cr.ProductId);

            modelBuilder.Entity<Order>()
                .HasOne(or => or.Users)
                .WithMany(us => us.Order)
                .HasForeignKey(or => or.UserId);

            modelBuilder.Entity<OrderItems>()
                .Property (pr => pr.Price)
                .HasPrecision(18, 2);

            modelBuilder.Entity<OrderItems>()
                .HasOne(ort => ort.Order)
                .WithMany(or => or.OrderItems)
                .HasForeignKey(ort => ort.OrderId);

            modelBuilder.Entity<OrderItems>()
                .HasOne(or => or.Products)
                .WithMany(pr => pr.OrderItems)
                .HasForeignKey(or => or.ProductId);


            modelBuilder.Entity<WhishList>()
                .HasOne(wh => wh.Users)
                .WithMany(us => us.WhishList)
                .HasForeignKey(wh => wh.UserId);

            modelBuilder.Entity<WhishList>()
                .HasOne(wh => wh.Products)
                .WithMany(pr => pr.WhishList)
                .HasForeignKey(wh => wh.ProductId);
            modelBuilder.Entity<Category>()
                .HasData(new Category { Id = 1, CategoryName = "Men" }, new Category { Id = 2, CategoryName = "Women" }
                );

                
        }
    }
}
