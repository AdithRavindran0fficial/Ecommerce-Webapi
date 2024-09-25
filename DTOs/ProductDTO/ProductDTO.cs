using System.ComponentModel.DataAnnotations;

namespace Ecommerce_Webapi.DTOs.ProductDTO
{
    public class ProductDTO
    {
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string Img { get; set; }
        [Required]
        public decimal Price { get; set; }

        [Required]
        public int CategoryId { get; set; }
    }
}
