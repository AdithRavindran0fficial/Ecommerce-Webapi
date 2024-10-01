﻿using Ecommerce_Webapi.DTOs.OrderDTO;
using Ecommerce_Webapi.Responses;
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
                return Ok(new ApiResponse<string>(200, "order placed"));
            }
            catch(Exception ex)
            {
                return StatusCode(500, new ApiResponse<string>(500,"Internal Server Error",null,ex.Message));
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
                return Ok(new ApiResponse<IEnumerable<OutOrders>>(200,"Success",details));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<string>(500, "Internal Server Error", null, ex.Message));
            }

        }
        [HttpGet("UserOrders")]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult>GetOrdersAdmin(int id)
        {
            try
            {
                var res = await orderservice.GetAllOrdersAdmin(id);
                return Ok(new ApiResponse<IEnumerable<OutOrders>>(200, "Success", res));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<string>(500, "Internal Server Error", null, ex.Message));
            }
        }
        [HttpGet("TotalRev")]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> GetRev()
        {
            try
            {
                var res = await orderservice.TotalRevenue();
                return Ok(new ApiResponse<decimal>(200, "Success", res));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<string>(500, "Internal Server Error", null, ex.Message));
            }
        }
        [HttpGet("TotalSales")]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> GetSale()
        {
            try
            {
                var res = await orderservice.TotalProductPurchased();
                return Ok(new ApiResponse<int>(200, "Success", res));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<string>(500, "Internal Server Error", null, ex.Message));
            }
        }
    }
}
