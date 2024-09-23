using System.ComponentModel.DataAnnotations;

namespace Ecommerce_Webapi.DTOs.CategoryDTO
{
    public class CategoryDTO
    {
        [Required]
        public string CategoryName { get; set; }
    }
}
