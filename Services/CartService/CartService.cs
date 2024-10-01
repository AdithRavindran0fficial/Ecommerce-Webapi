using Ecommerce_Webapi.Data;
using Ecommerce_Webapi.DTOs.CartDTO;
using Ecommerce_Webapi.DTOs.CartItemDTO;
using Ecommerce_Webapi.Models;
using Ecommerce_Webapi.Services.JWTServices;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce_Webapi.Services.CartService
{
    public class CartService :ICartService
    {
        private AppDbContext _context;
        private readonly IJWTServices jwtservice;
        private IConfiguration _configuration;
        public CartService(IJWTServices jwtservice, AppDbContext context,IConfiguration configuration)
        {
            this.jwtservice = jwtservice;
            this._context = context;
            this._configuration = configuration;
        }
        public async Task<IEnumerable<OutCart>> GetAllItems(string token)
        {
            try
            {
                var userid = jwtservice.GetUserId(token);
                
                var user = await _context.Users.Include(u => u.Cart)
                    .ThenInclude(cart => cart.CartItems)
                    .ThenInclude(cati => cati.Products)
                    .FirstOrDefaultAsync(us => us.Id == userid);
                if (user == null)
                {
                    throw new Exception("User not found");
                }
                if (user.Cart == null || !user.Cart.CartItems.Any())
                {
                    return new List<OutCart>();
                }
                var items = user.Cart.CartItems.Select(itm => new OutCart
                {
                    Id = itm.ProductId,
                    Title = itm.Products.Title,
                    Description = itm.Products.Description,
                    Img = $"{_configuration["HostUrl:images"]}/Products/{itm.Products.Img}",
                    Price =itm.Products.Price * itm.Quantity,
                    Quantity = itm.Quantity,
                    Total = itm.Quantity * itm.Products.Price
                }).ToList();
                return items;



            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<bool> AddToCart(string token, int productid)
        {
            try
            {
                var userid = jwtservice.GetUserId(token);
                var user = await _context.Users.Include(c => c.Cart).
                    ThenInclude(ct => ct.CartItems).
                    ThenInclude(pr => pr.Products).
                    FirstOrDefaultAsync(us => us.Id == userid);
                if (user == null)
                {
                    throw new Exception("User not found");
                }




                if (user.Cart == null)
                {
                    user.Cart = new Cart { UserId=userid,CartItems=new List<CartItem>() };
                    _context.Carts.Add(user.Cart);
                    await _context.SaveChangesAsync();
                }






                var check = user.Cart.CartItems.FirstOrDefault(ct => ct.ProductId == productid);
                if (check != null)
                {
                    return false;

                }
                var item = new CartItem
                {
                    CartId = user.Cart.Id,
                    ProductId = productid,
                    Quantity = 1
                };
                user.Cart.CartItems.Add(item);
                await _context.SaveChangesAsync();
                return true;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<bool> RemoveCart(string token, int productid)
        {
            try
            {
                var userid = jwtservice.GetUserId(token);
                var user = await _context.Users.Include(ct => ct.Cart)
                            .ThenInclude(cti => cti.CartItems).
                            ThenInclude(pr => pr.Products).
                            FirstOrDefaultAsync(us => us.Id == userid);
                if (user == null)
                {
                    throw new Exception("user not found");
                }
                var deletItem = user.Cart.CartItems.FirstOrDefault(pr => pr.ProductId == productid);
                if (deletItem == null)
                {
                    return false;

                }
                user.Cart.CartItems.Remove(deletItem);
                await _context.SaveChangesAsync();
                return true;               
           }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<bool> IncreaseQty(string token, int productid)
        {
            try
            {
                var userid = jwtservice.GetUserId(token);
                var user = await _context.Users.Include(ct => ct.Cart)
                            .ThenInclude(cti => cti.CartItems).
                            ThenInclude(pr => pr.Products).
                            FirstOrDefaultAsync(us => us.Id == userid);
                if (user == null)
                {
                    throw new Exception("user not found");
                }
                var item = user.Cart.CartItems.FirstOrDefault(pr => pr.ProductId == productid);
                if(item == null)
                {
                    return false;
                }
                item.Quantity = item.Quantity + 1;
                await _context.SaveChangesAsync();
                return true;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<bool> DecreaseQty(string token, int productid)
        {
            try
            {
                var userid = jwtservice.GetUserId(token);
                var user = await _context.Users.Include(ct => ct.Cart)
                            .ThenInclude(cti => cti.CartItems).
                            ThenInclude(pr => pr.Products).
                            FirstOrDefaultAsync(us => us.Id == userid);
                if (user == null)
                {
                    throw new Exception("User not found");
                }
                var item = user.Cart.CartItems.FirstOrDefault(pr => pr.ProductId == productid);
                if (item == null)
                {
                    return false;
                }
                item.Quantity = item.Quantity - 1;
                if (item.Quantity < 1)
                {
                    user.Cart.CartItems.Remove(item);
                }
                await _context.SaveChangesAsync();
                return true;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }





    }
}

