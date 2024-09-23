using Ecommerce_Webapi.DTOs.CategoryDTO;
using Ecommerce_Webapi.DTOs.ProductDTO;

namespace Ecommerce_Webapi.Services.ProductService
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDTO>> GetAllProduct();
        Task<ProductDTO> GetProductById(int id);
        Task<IEnumerable<ProductDTO>> GetProductByCat(CategoryDTO category);
        Task<bool> AddProduct(ProductDTO product);
        Task<bool> UpdateProduct(ProductDTO product);
        Task<bool> DeleteProduct(int id);
    }
}
