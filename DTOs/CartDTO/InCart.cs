using System.ComponentModel.DataAnnotations;

namespace Ecommerce_Webapi.DTOs.CartDTO
{
    public class InCart
    {
        [Required]
        public int ProductId { get; set; }
    }
}
