using Ecommerce_Webapi.DTOs.CategoryDTO;
using Ecommerce_Webapi.DTOs.ProductDTO;
using Ecommerce_Webapi.Services.ProductService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce_Webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private IProductService productService;
        public ProductController(IProductService products)
        {
            this.productService = products;
        }
        [HttpGet("All")]
        public async Task<IActionResult> GetAll()
        {
            try
            {

                var products = await  productService.GetAllProduct();
                return Ok(products);

            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server issue:{ex.Message}");
            } 
        }
        [HttpGet("product/{id}")]
        public async Task<IActionResult> GetbyId(int id)
        {
            try
            {
                var product = await productService.GetProductById(id);
                if (product == null)
                {
                    return BadRequest("Employee not found");
                }
                return Ok(product);
            }
            catch(Exception ex)
            {
                return StatusCode(500, $"Internal server issue:{ex.Message}");
            }
        }
        [HttpGet("ProductByCategory/{category}")]
        public async Task<IActionResult> GetBycat(CategoryDTO category)
        {
            try
            {
                var product = await productService.GetProductByCat(category);
                return Ok(product);
            }
            catch(Exception ex)
            {
                return StatusCode(500, $"Internal server issue:{ex.Message}");
            }
        }
        [HttpGet("Search/{name}")]
        public async Task<IActionResult>GetBySearch(string name)
        {
            try
            {
                var product = await productService.Search(name);
                return Ok(product);
            }
            catch(Exception ex)
            {
                return StatusCode(500, $"Internal server issue:{ex.Message}");
            }
        }
        
        [HttpPost("AddProduct")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult>Addproduct(ProductDTO product)
        {
            try
            {
                var res = await productService.AddProduct(product);
                return Ok("successfully added");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server issue:{ex.Message}");
            }
        }
        
        [HttpPost("UpdateProduct/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id, ProductDTO product)
        {
            try
            {
                var res = await productService.UpdateProduct(id, product);
                if (res)
                {

                    return Ok("Successfully updated");
                }
                return BadRequest("Product Notfound");

            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server issue:{ex.Message}");
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
                    return Ok("Successfully deleted");

                }
                return BadRequest("Product not found");
            }
            catch(Exception ex)
            {
                return StatusCode(500, $"Internal server issue:{ex.Message}");
            }
        }
        

       
    }
}
