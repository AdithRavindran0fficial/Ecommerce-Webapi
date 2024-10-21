using Ecommerce_Webapi.DTOs.CartDTO;

namespace Ecommerce_Webapi.Services.CartService
{
    public interface ICartService
    {
        Task<IEnumerable<OutCart>> GetAllItems(int token);
        Task<bool> AddToCart(int token,InCart product);
        Task<bool> RemoveCart(int token, int productid);
        Task<bool> IncreaseQty(int token, InCart product);
        Task<bool> DecreaseQty(int token, InCart product);

    }
}
