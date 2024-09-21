using System.ComponentModel.DataAnnotations;

namespace Ecommerce_Webapi.Models.UserModel
{
    public class Login
    {
        [Required]
        public string Email { get; set; }
        public string Password { get; set; }   
    }
}
