using System.ComponentModel.DataAnnotations;

namespace Ecommerce_Webapi.Models
{
    public class Products
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        
        public decimal Price { get; set; }
        [Required]
        public string Img { get; set; }
        [Required]
        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }
        public ICollection<CartItem> CartItems { get; set; }
        public ICollection<OrderItems> OrderItems { get; set; }
        public ICollection<WhishList> WhishList { get; set; }
    }
}
