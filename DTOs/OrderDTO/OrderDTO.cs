using System.ComponentModel.DataAnnotations;

namespace Ecommerce_Webapi.DTOs.OrderDTO
{
    public class OrderDTO
    {
        [Required]
        public int UserId { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        [EmailAddress]
        public string UserEmail { get; set; }
        [Required]
        public int UserPhone { get; set; }
        [Required]
        public string UserAddress { get; set; }
        [Required]
        public DateTime OrderDate { get; set; }
    }
}
