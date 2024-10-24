﻿using Ecommerce_Webapi.Data;
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
        public async Task<IEnumerable<OutCart>> GetAllItems(int id)
        {
            try
            {
                
                
                var user = await _context.Users.Include(u => u.Cart)
                    .ThenInclude(cart => cart.CartItems)
                    .ThenInclude(cati => cati.Products)
                    .FirstOrDefaultAsync(us => us.Id == id);
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
                    Price =itm.Products.Price,
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
        public async Task<bool> AddToCart(int id, InCart product)
        {
            try
            {
                
                var user = await _context.Users.Include(c => c.Cart).
                    ThenInclude(ct => ct.CartItems).
                    ThenInclude(pr => pr.Products).
                    FirstOrDefaultAsync(us => us.Id == id);
                if (user == null)
                {
                    throw new Exception("User not found");
                }




                if (user.Cart == null)
                {
                    user.Cart = new Cart { UserId=id,CartItems=new List<CartItem>() };
                    _context.Carts.Add(user.Cart);
                    await _context.SaveChangesAsync();
                }






                var check = user.Cart.CartItems.FirstOrDefault(ct => ct.ProductId == product.ProductId);
                if (check != null)
                {
                    return false;

                }
                var item = new CartItem
                {
                    CartId = user.Cart.Id,
                    ProductId = product.ProductId,
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
        public async Task<bool> RemoveCart(int id, int productid)
        {
            try
            {
                
                var user = await _context.Users.Include(ct => ct.Cart)
                            .ThenInclude(cti => cti.CartItems).
                            ThenInclude(pr => pr.Products).
                            FirstOrDefaultAsync(us => us.Id == id);
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
        public async Task<bool> IncreaseQty(int id, InCart product)
        {
            try
            {
                
                var user = await _context.Users.Include(ct => ct.Cart)
                            .ThenInclude(cti => cti.CartItems).
                            ThenInclude(pr => pr.Products).
                            FirstOrDefaultAsync(us => us.Id == id);
                if (user == null)
                {
                    throw new Exception("user not found");
                }
                var item = user.Cart.CartItems.FirstOrDefault(pr => pr.ProductId == product.ProductId);
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
        public async Task<bool> DecreaseQty(int id, InCart product)
        {
            try
            {
                
                var user = await _context.Users.Include(ct => ct.Cart)
                            .ThenInclude(cti => cti.CartItems).
                            ThenInclude(pr => pr.Products).
                            FirstOrDefaultAsync(us => us.Id == id);
                if (user == null)
                {
                    throw new Exception("User not found");
                }
                var item = user.Cart.CartItems.FirstOrDefault(pr => pr.ProductId == product.ProductId);
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

