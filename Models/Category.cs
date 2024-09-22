using System.ComponentModel.DataAnnotations;

namespace Ecommerce_Webapi.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string CategoryName { get; set; }
        public ICollection<Products> Products { get; set; }
    }
}
