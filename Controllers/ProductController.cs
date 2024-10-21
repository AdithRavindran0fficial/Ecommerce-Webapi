using Ecommerce_Webapi.ActionFilters;
using Ecommerce_Webapi.Data;
using Ecommerce_Webapi.DTOs.CategoryDTO;
using Ecommerce_Webapi.DTOs.ProductDTO;
using Ecommerce_Webapi.Models;
using Ecommerce_Webapi.Responses;
using Ecommerce_Webapi.Services.ProductService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;

namespace Ecommerce_Webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class ProductController : ControllerBase
    {
        private IProductService productService;
        private AppDbContext _context;
        public ProductController(IProductService products,AppDbContext context)
        {
            this.productService = products;
            _context = context;
        }
        [HttpGet("All")]
        [TimeCalculation]
        
        public async Task<IActionResult> GetAll()
        {
            try
            {

                var products = await productService.GetAllProduct();
                var resp = new ApiResponse<IEnumerable<ProductViewDTO>>(200, "Ok", products);
                return Ok(resp);

            }
            catch (Exception ex)
            {
                var resp = new ApiResponse<string>(500, "Internal server error", null, ex.Message);
                return StatusCode(500, resp);
            }
        }
        [HttpGet("product/{id}")]
        public async Task<IActionResult> GetbyId(int id)
        {
            try
            {
                var product = await productService.GetProductById(id);
                var resp = new ApiResponse<ProductViewDTO>(200, "Ok", product);
                return Ok(resp);
            }
            catch (Exception ex)
            {
                var resp = new ApiResponse<string>(500, "Internal server error", null, ex.Message);
                return StatusCode(500, resp);
            }
        }
        [HttpGet("{category}")]
        public  IActionResult GetBycat( string category)
        {
            try
            {
                var product =  productService.GetProductByCat(category);
                var resp = new ApiResponse<IEnumerable<ProductViewDTO>>(200, "Ok", product);
                return Ok(resp);
            }
            catch(Exception ex)
            {
                var resp = new ApiResponse<string>(500, "Internal server error", null, ex.Message);
                return StatusCode(500, resp);
            }
        }
        [HttpGet("Search/{name}")]
        public IActionResult GetBySearch(string name)
        {
            try
            {
                var product =  productService.Search(name);
                var resp = new ApiResponse<IEnumerable<ProductViewDTO>>(200, "Ok", product);
                return Ok(resp);
            }
            catch(Exception ex)
            {
                var resp = new ApiResponse<string>(500, "Internal server error", null, ex.Message);
                return StatusCode(500, resp);
            }
        }
        
        [HttpPost("AddProduct")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult>Addproduct([FromForm] Addproduct product,IFormFile img)
        {
            try
            {
                var res = await productService.AddProduct(product,img);
                if (res)
                {
                    return Ok(new ApiResponse<bool>(200, "successfully added", res, null));
                }
                return BadRequest(new ApiResponse<bool>(400, "product already exist or category not available", res));
                
            }
            catch (Exception ex)
            {
                var resp = new ApiResponse<string>(500, "Internal server error", null, ex.Message);
                return StatusCode(500, resp);
            }
        }
        
        [HttpPut("UpdateProduct/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id, [FromForm] Addproduct product,IFormFile img)
        {
            try
            {
                var cat = _context.Categories.FirstOrDefault(c => c.Id == product.CategoryId);
                if (cat == null)
                {
                    return BadRequest($"Category not found{product.CategoryId}");
                }
                var res = await productService.UpdateProduct(id, product,img);
                if (res)
                {

                    return Ok(new ApiResponse<bool>(200, "successfully updated", res, null));
                }
                return BadRequest(new ApiResponse<string>(400, "Product Notfound"));

            }
            catch (Exception ex)
            {
                var resp = new ApiResponse<string>(500, "Internal server error", null, ex.Message);
                return StatusCode(500, resp);
            }
        }
        [HttpDelete("Delete/{id}")]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult>DeleteProduct(int id)
        {
            try
            {
                var res = await productService.DeleteProduct(id);
                if (res)
                {
                    return Ok(new ApiResponse<bool>(200, "Successfully deleted",res));

                }
                return BadRequest(new ApiResponse<bool>(404, "Product not found",res));
            }
            catch(Exception ex)
            {
                var resp = new ApiResponse<string>(500, "Internal server error", null, ex.Message);
                return StatusCode(500, resp);
            }
        }
        

       
    }
}
