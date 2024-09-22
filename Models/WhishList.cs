using Ecommerce_Webapi.Models.UserModel;
using System.ComponentModel.DataAnnotations;

namespace Ecommerce_Webapi.Models
{
    public class WhishList
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int UserId { get; set; }
        [Required]
        public int ProductId { get; set; }
        public virtual Users Users { get; set; }
        public virtual Products Products { get; set; }

    }
}
