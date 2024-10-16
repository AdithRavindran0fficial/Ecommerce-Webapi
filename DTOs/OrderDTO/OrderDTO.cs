using System.ComponentModel.DataAnnotations;

namespace Ecommerce_Webapi.DTOs.OrderDTO
{
    public class OrderDTO
    {
        public string UserName { get; set; }
        [Required]
        [EmailAddress]
        public string UserEmail { get; set; }
        [Required]
        public string UserPhone { get; set; }
        [Required]
        public string UserAddress { get; set; }
        //public string OrderString { get; set; }

        //public string TransactionId { get; set; }
        [Required]
        public DateTime OrderDate { get; set; }
    }
}
