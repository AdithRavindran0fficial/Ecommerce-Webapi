using Ecommerce_Webapi.DTOs.OrderDTO;
using Ecommerce_Webapi.Services.OrderService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce_Webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private IOrderService orderservice;
        public OrderController(IOrderService orderservice)
        {
            this.orderservice = orderservice;
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult>OrderPlace(OrderDTO orderDTO)
        {
            try
            {
                var auth = HttpContext.Request.Headers["Authorization"].FirstOrDefault().Split();
                var token = auth[1];
                var res = await orderservice.OrderPlace(token, orderDTO);
                return Ok("order placed");
            }
            catch(Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpGet("getOrder")]
        [Authorize]
        public async Task<IActionResult> GetOrder()
        {
            try
            {
                var auth = HttpContext.Request.Headers["Authorization"].FirstOrDefault().Split();
                var token = auth[1];
                var details = await orderservice.GetOrderDetail(token);
                return Ok(details);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }
        [HttpGet("UserOrders")]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult>GetOrdersAdmin(int id)
        {
            try
            {
                var res = await orderservice.GetAllOrdersAdmin(id);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpGet("TotalRev")]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> GetRev()
        {
            try
            {
                var res = await orderservice.TotalRevenue();
                return Ok(res);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpGet("TotalSales")]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> GetSale()
        {
            try
            {
                var res = orderservice.TotalProductPurchased();
                return Ok(res);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
