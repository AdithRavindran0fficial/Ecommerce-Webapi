using Ecommerce_Webapi.DTOs.WhishListDTO;

namespace Ecommerce_Webapi.Services.WhishListService
{
    public interface IWhishList
    {
        Task<bool> AddToWhishList(string token, int productid);
        Task<IEnumerable< OutWhishList>> GetItems(string token);
        Task<bool>RemoveWhishlist(string token,int productid);

    }
}
