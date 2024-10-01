using Ecommerce_Webapi.DTOs.CartDTO;
using Ecommerce_Webapi.Responses;
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
                return Ok (new ApiResponse<IEnumerable<OutCart>>(200,"Sucessfully fetched",cart));

            }
            catch (Exception ex)
            {
                var resp = new ApiResponse<string>(500,"Internal Server Error Occured",null,ex.Message);
                return StatusCode(500,resp);
            }
        }
        [HttpPost("Addtocart")]
        [Authorize]
        public async Task<IActionResult>Addtocart(int productid)
        {
            try
            {

                var token = HttpContext.Request.Headers["Authorization"].FirstOrDefault().Split();
                var jwt = token[1];
                var res = await _cartService.AddToCart(jwt, productid);
                if (res == false)
                {
                    return BadRequest(new ApiResponse<bool>(400,"Item already in cart",res));

                }
                return Ok(new ApiResponse<bool>(200, "SuccessFully added", res));

            }
            catch (Exception ex) 
            {
                var resp = new ApiResponse<string>(500, "Internal Server Error Occured", null, ex.Message);
                return StatusCode(500, resp);

            }


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
                return BadRequest(new ApiResponse<bool>(400, "Item not found in cart",res));
            }
            return Ok(new ApiResponse<bool>(200, "successfully removed",res));

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
                return BadRequest(new ApiResponse<bool>(400, "Item not found in cart", res));
            }
            return Ok(new ApiResponse<bool>(200, "successfully increased", res));
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
                return BadRequest(new ApiResponse<bool>(400, "Item not found in cart", res));
            }
            return Ok(new ApiResponse<bool>(200, "successfully decreased", res));
        }

    }
    }
