using Ecommerce_Webapi.DTOs.WhishListDTO;
using Ecommerce_Webapi.Responses;
using Ecommerce_Webapi.Services.WhishListService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce_Webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WhishListController : ControllerBase
    {
        private IWhishList whishlist;
        public WhishListController(IWhishList wishlist) 
        {
            whishlist= wishlist;
        
        }
        [HttpGet("all")]
        [Authorize]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var auth = HttpContext.Request.Headers["Authorization"].FirstOrDefault().Split();
                var token = auth[1];
                var items = await whishlist.GetItems(token);
                return Ok(new ApiResponse<IEnumerable<OutWhishList>>(200,"Successfully fetched", items));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<string>(500,"Internal Server Error",null,ex.Message));
            }
        }
        [HttpPost("Add")]
        [Authorize]
        public async Task<IActionResult>Addto(int productid)
        {
            try
            {
                var auth = HttpContext.Request.Headers["Authorization"].FirstOrDefault().Split();
                var token = auth[1];
                var resp = await whishlist.AddToWhishList(token, productid);
                return Ok(new ApiResponse<bool>(200,"successfully added",resp));

            }
            catch(Exception ex)
            {
                return StatusCode(500, new ApiResponse<string>(500, "Internal Server Error", null, ex.Message));
            }
        }
        [HttpDelete("Remove")]
        [Authorize]
        public async Task<IActionResult>Remove(int id)
        {
            try
            {

                var auth = HttpContext.Request.Headers["Authorization"].FirstOrDefault().Split();
                var token = auth[1];
                var resp = await whishlist.RemoveWhishlist(token, id);
                if (resp)
                {
                    return Ok(new ApiResponse<bool>(200, "successfully Removed", resp));
                }
                return NotFound(new ApiResponse<bool>(200, "item not found", resp));
            }
            catch(Exception ex)
            {
                return StatusCode(500, new ApiResponse<string>(500, "Internal Server Error", null, ex.Message));
            }
        }
    }
}
