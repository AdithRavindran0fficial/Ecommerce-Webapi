using System.ComponentModel.DataAnnotations;

namespace Ecommerce_Webapi.DTOs
{
    public class UserDTO
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string UserEmail { get; set; }
        [Required]
        [StringLength(100, MinimumLength = 8)]
        public string Password { get; set; }
    }
}
