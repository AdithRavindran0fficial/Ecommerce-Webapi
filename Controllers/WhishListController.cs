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
                return Ok(items);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
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
                return Ok(resp);

            }
            catch(Exception ex)
            {
                return StatusCode(500, ex.Message);
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
                    return Ok("succesfully removed");
                }
                return NotFound("item not found");
            }
            catch(Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
