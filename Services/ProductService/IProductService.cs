using Ecommerce_Webapi.DTOs.CategoryDTO;
using Ecommerce_Webapi.DTOs.ProductDTO;

namespace Ecommerce_Webapi.Services.ProductService
{
    public interface IProductService
    {
        Task<IEnumerable<ProductViewDTO>> GetAllProduct();
        Task<ProductViewDTO> GetProductById(int id);
        Task<IEnumerable<ProductViewDTO>> GetProductByCat(CategoryDTO category);
        Task<IEnumerable<ProductViewDTO>> Search(string name);
        Task<bool> AddProduct(ProductDTO product);
        Task<bool> UpdateProduct(int id,ProductDTO product);
        Task<bool> DeleteProduct(int id);
    }
}
