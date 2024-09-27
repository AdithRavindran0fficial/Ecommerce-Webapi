using System.ComponentModel.DataAnnotations;

namespace Ecommerce_Webapi.DTOs.WhishListDTO
{
    public class AddWhishListDTO
    {
        public int UserId { get; set; }
        
        public int ProductId { get; set; }
    }
}
