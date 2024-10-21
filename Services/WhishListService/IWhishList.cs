using Ecommerce_Webapi.DTOs.WhishListDTO;

namespace Ecommerce_Webapi.Services.WhishListService
{
    public interface IWhishList
    {
        Task<bool> AddToWhishList(int Userid, int productid);
        Task<IEnumerable< OutWhishList>> GetItems(int Userid);
        Task<bool>RemoveWhishlist(int Userid,int productid);

    }
}
