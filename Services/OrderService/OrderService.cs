using Ecommerce_Webapi.Data;
using Ecommerce_Webapi.DTOs.OrderDTO;
using Ecommerce_Webapi.Models;
using Ecommerce_Webapi.Services.JWTServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce_Webapi.Services.OrderService
{
    public class OrderService : IOrderService
    {
        private IJWTServices JWTServices;
        private AppDbContext _context;
        public OrderService(IJWTServices jwt, AppDbContext context)
        {
            JWTServices = jwt;
            _context = context;
        }
        public async Task<bool> OrderPlace(string token, OrderDTO orderDTO)
        {
            try
            {
                var userid = JWTServices.GetUserId(token);
                var user = await _context.Users.Include(ct => ct.Cart)
                    .ThenInclude(cti => cti.CartItems)
                    .ThenInclude(pr => pr.Products)
                    .FirstOrDefaultAsync(us => us.Id == userid);
                if (user == null || user.Cart.CartItems == null || !user.Cart.CartItems.Any())
                {

                    throw new Exception("cart is empty order cannnot be placed");

                }
                decimal total = user.Cart.CartItems.Sum(cartitm => cartitm.Products.Price * cartitm.Quantity);
                Order order = new Order
                {
                    UserId = userid,
                    UserAddress = orderDTO.UserAddress,
                    UserPhone = orderDTO.UserPhone,
                    OrderDate = DateTime.Now,
                    total = total,
                    OrderItems = user.Cart.CartItems.Select(ct => new OrderItems
                    {
                        ProductId = ct.ProductId,
                        ProductName = ct.Products.Title,
                        Quantity = ct.Quantity,
                        Price = ct.Quantity * ct.Products.Price
                    }).ToList()

                };
                _context.Orders.Add(order);
                await _context.SaveChangesAsync();

                foreach (var item in user.Cart.CartItems)
                {
                    _context.CartItems.Remove(item);
                }
                await _context.SaveChangesAsync();
                return true;



            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<IEnumerable<OutOrders>> GetOrderDetail(string token)
        {
            try
            {
                var userid = JWTServices.GetUserId(token);
                var user = await _context.Orders.Include(or => or.OrderItems).ThenInclude(pr=>pr.Products).FirstOrDefaultAsync(user => user.UserId == userid);
                if (user == null || !user.OrderItems.Any())
                {
                    return new List<OutOrders>();
                }
               
                var details = user.OrderItems.Select(item => new OutOrders
                {
                    Id = item.Id,
                    Product_Id = item.ProductId,
                    Product_Name = item.ProductName,
                    Qty = item.Quantity,
                    Total = item.Price


                }).ToList();
                return details;

            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }            
        }
        public async Task<List<OutOrders>> GetAllOrdersAdmin(int id)
        {
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(us => us.Id == id);
                if (user == null)
                {
                    throw new Exception("User not found");
                }
                var orders = await _context.Orders.Include(or => or.OrderItems)
                    .ThenInclude(itm => itm.Products)
                    .Where(us => us.UserId == id).ToListAsync();
                if (orders == null)
                {
                    return new List<OutOrders>();
                }
                List<OutOrders> details = new List<OutOrders>();
                foreach(var order in orders)
                {
                    foreach(var item in order.OrderItems)
                    {
                        details.Add(new OutOrders
                        {
                            Id = item.Id,
                            Product_Id = item.ProductId,
                            //Price = item.Price,
                            Product_Name = item.ProductName,
                            Qty = item.Quantity,
                            Total = item.Price * item.Quantity
                        });
                    }
                }
                return details;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<decimal> TotalRevenue()
        {
            try
            {
                var total = await _context.OrderItems.SumAsync(itm => itm.Price * itm.Quantity);
                return total;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<int> TotalProductPurchased()
        {
            try
            {
                var total = await _context.OrderItems.SumAsync(or => or.Quantity);
                return total;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
