using Ecommerce_Webapi.DTOs.CategoryDTO;
using Ecommerce_Webapi.DTOs.ProductDTO;

namespace Ecommerce_Webapi.Services.ProductService
{
    public interface IProductService
    {
        Task<IEnumerable<ProductViewDTO>> GetAllProduct();
        Task<ProductViewDTO> GetProductById(int id);
        IEnumerable<ProductViewDTO> GetProductByCat(string category);
        IEnumerable<ProductViewDTO> Search(string name);
        Task<bool> AddProduct(Addproduct product,IFormFile img);
        Task<bool> UpdateProduct(int id, Addproduct product,IFormFile img);
        Task<bool> DeleteProduct(int id);
    }
}
