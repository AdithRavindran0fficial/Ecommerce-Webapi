using System.ComponentModel.DataAnnotations;

namespace Ecommerce_Webapi.DTOs
{
    public class UserDTO
    {
        [Required]
        public string User_Name { get; set; }
        [Required]
        public string User_Email { get; set; }
        [Required]
        [StringLength(100, MinimumLength = 8)]
        public string User_Password { get; set; }
    }
}
