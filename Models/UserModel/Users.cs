using System.ComponentModel.DataAnnotations;

namespace Ecommerce_Webapi.Models.UserModel
{
    public class Users
    {
        public int Id { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        public string UserEmail { get; set; }
        [Required]
        [StringLength(100,MinimumLength =8)]
        public string Password { get; set; }
        public bool IsStatus { get; set; } = true;

        public string Role { get; set; } = "User";
    }
}
