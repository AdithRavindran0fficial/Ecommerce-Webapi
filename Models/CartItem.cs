﻿using Ecommerce_Webapi.Models.UserModel;
using System.ComponentModel.DataAnnotations;

namespace Ecommerce_Webapi.Models
{
    public class CartItem
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int CartId {  get; set; }
        [Required]
        public int ProductId { get; set; }
        [Required]
        public int Quantity { get; set; }
        public virtual Cart Cart { get; set; }
        public virtual Users Users { get; set; }

    }
}
