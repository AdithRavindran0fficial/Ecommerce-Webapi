using Ecommerce_Webapi.Services.CartService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce_Webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private ICartService _cartService;
        public CartController(ICartService cartservice)
        {
            _cartService = cartservice;

        }
        [HttpGet("Cart")]
        [Authorize]
        public async Task<IActionResult> Get()
        {
            try
            {
                var token = HttpContext.Request.Headers["Authorization"].FirstOrDefault().Split();
                var jwt = token[1];

                var cart = await _cartService.GetAllItems(jwt);
                return Ok (cart);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("Addtocart")]
        [Authorize]
        public async Task<IActionResult>Addtocart(int productid)
        {
            
                var token = HttpContext.Request.Headers["Authorization"].FirstOrDefault().Split();
                var jwt = token[1];
                var res = await _cartService.AddToCart(jwt, productid);
                if (res == false) 
                {
                    return BadRequest("Item already in cart");

                }
                return Ok("Successfully added");

            }
        [HttpDelete("Remove")]
        [Authorize]
        public async Task<IActionResult>Remove(int productid)
        {
            var token = HttpContext.Request.Headers["Authorization"].FirstOrDefault().Split();
            var jwt = token[1];
            var res = await _cartService.RemoveCart(jwt,productid);
            if(res == false)
            {
                return BadRequest("Item not found in cart");
            }
            return Ok("successfully removed");

        }
        [HttpPut("increaseqty")]
        [Authorize]
        public async Task<IActionResult>Increaseqty(int productid)
        {
            var token = HttpContext.Request.Headers["Authorization"].FirstOrDefault().Split();
            var jwt = token[1];
            var res = await _cartService.IncreaseQty(jwt, productid);
            if (res == false)
            {
                return BadRequest("Item not found");
            }
            return Ok("increased");
        }
        [HttpPut("decreaseqty")]
        [Authorize]
        public async Task<IActionResult> Decreaseqty(int productid)
        {
            var token = HttpContext.Request.Headers["Authorization"].FirstOrDefault().Split();
            var jwt = token[1];
            var res = await _cartService.DecreaseQty(jwt, productid);
            if (res == false)
            {
                return BadRequest("Item not found");
            }
            return Ok("increased");
        }

    }
    }
