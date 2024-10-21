using Ecommerce_Webapi.Data;
using Ecommerce_Webapi.DTOs.OrderDTO;
using Ecommerce_Webapi.Models;
using Ecommerce_Webapi.Services.JWTServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Razorpay.Api;

namespace Ecommerce_Webapi.Services.OrderService
{
    public class OrderService : IOrderService
    {
        private IJWTServices JWTServices;
        private AppDbContext _context;
        private IConfiguration _configuration;
        public OrderService(IJWTServices jwt, AppDbContext context,IConfiguration configuration)
        {
            JWTServices = jwt;
            _context = context;
            _configuration = configuration;
        }

        public async Task<string> OrderCreate(long price)
        {
            Dictionary<string, object> input = new Dictionary<string, object>();
            Random random = new Random();
            string TrasactionId = random.Next(0, 1000).ToString();
            input.Add("amount", Convert.ToDecimal(price) * 100);
            input.Add("currency", "INR");
            input.Add("receipt", TrasactionId);

            string key = _configuration["Razorpay:KeyId"];
            string secret = _configuration["Razorpay:KeySecret"];

            RazorpayClient client = new RazorpayClient(key, secret);

            Razorpay.Api.Order order = client.Order.Create(input);
            var OrderId = order["id"].ToString();

            return OrderId;
        }
        public bool Payment(PaymentDto razorpay)
        {
            if (razorpay == null ||
                string.IsNullOrEmpty(razorpay.razorpay_payment_id) ||
                string.IsNullOrEmpty(razorpay.razorpay_order_id) ||
                string.IsNullOrEmpty(razorpay.razorpay_signature))
            {
                return false;
            }

            try
            {
                RazorpayClient client = new RazorpayClient(
                    _configuration["Razorpay:KeyId"],
                    _configuration["Razorpay:KeySecret"]
                );

                Dictionary<string, string> attributes = new Dictionary<string, string>
        {
            { "razorpay_payment_id", razorpay.razorpay_payment_id },
            { "razorpay_order_id", razorpay.razorpay_order_id },
            { "razorpay_signature", razorpay.razorpay_signature }
        };

                Utils.verifyPaymentSignature(attributes);
                return true;
            }
            catch (Exception ex)
            {
                
                throw new Exception("Payment verification failed: " + ex.Message);
            }
        }


        public async Task<bool> OrderPlace(int id, OrderDTO orderDTO)
        {
            try
            {
                //var userid = JWTServices.GetUserId(token);
                var user = await _context.Users.Include(ct => ct.Cart)
                    .ThenInclude(cti => cti.CartItems)
                    .ThenInclude(pr => pr.Products)
                    .FirstOrDefaultAsync(us => us.Id == id);
                if (user == null || user.Cart == null || user.Cart.CartItems == null || !user.Cart.CartItems.Any())
                {

                    return false;

                }
                decimal total = user.Cart.CartItems.Sum(cartitm => cartitm.Products.Price * cartitm.Quantity);
                Ecommerce_Webapi.Models.Order order = new Ecommerce_Webapi.Models.Order
                {
                    UserId = id,
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
        public async Task<IEnumerable<OutOrders>> GetOrderDetail(int id)
        {
            try
            {
                //var userid = JWTServices.GetUserId(token);
                var user = await _context.Orders.Include(or => or.OrderItems).ThenInclude(pr=>pr.Products).FirstOrDefaultAsync(user => user.UserId == id);
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
                    Total = item.Price,



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
