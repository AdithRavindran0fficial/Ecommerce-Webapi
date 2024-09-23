using System.ComponentModel.DataAnnotations;

namespace Ecommerce_Webapi.Models
{
    public class OrderItems
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int OrderId { get; set; }
        [Required]
        public int ProductId { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        public decimal Price { get; set; }
        public virtual Products Products { get; set; }
        public virtual Order Order { get; set; }

    }
}
