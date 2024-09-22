using Ecommerce_Webapi.Models.UserModel;
using System.ComponentModel.DataAnnotations;

namespace Ecommerce_Webapi.Models
{
    public class Cart
    {
        [Key]
       public int Id { get; set; }
        [Required]
       public int UserId { get; set; }
       public virtual Users Users { get; set; }
        public ICollection<CartItem> CartItems { get; set; }
    }
}
