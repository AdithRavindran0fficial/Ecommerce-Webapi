using Ecommerce_Webapi.DTOs.CartDTO;

namespace Ecommerce_Webapi.Services.CartService
{
    public interface ICartService
    {
        Task<IEnumerable<OutCart>> GetAllItems(string token);
        Task<bool> AddToCart(string token,int productid);
        Task<bool> RemoveCart(string token, int productid);
        Task<bool> IncreaseQty(string token, int productid);
        Task<bool> DecreaseQty(string token, int productid);

    }
}
