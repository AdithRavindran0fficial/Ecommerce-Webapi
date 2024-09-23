﻿using Ecommerce_Webapi.Models.UserModel;
using System.ComponentModel.DataAnnotations;

namespace Ecommerce_Webapi.Models
{
    public class Order
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int UserId { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        [EmailAddress]
        public string UserEmail { get; set; }
        [Required]
        public int UserPhone { get; set; }
        [Required]
        public string UserAddress { get; set; }
        [Required]
        public DateTime OrderDate { get; set; }
        public virtual Users Users { get; set; }
        public ICollection<OrderItems> OrderItems { get; set; }

    }
}