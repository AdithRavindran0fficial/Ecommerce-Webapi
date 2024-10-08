﻿using System.ComponentModel.DataAnnotations;

namespace Ecommerce_Webapi.Models.UserModel
{
    public class Users
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        [EmailAddress(ErrorMessage ="Please enter a valid email")]
        public string UserEmail { get; set; }
        [Required]
        [StringLength(100,MinimumLength =8)]
        public string Password { get; set; }
        public string ? Phone {  get; set; }
        public bool IsStatus { get; set; } = true;
        public string Role { get; set; } = "User";
        public virtual Cart Cart { get; set; }
        public ICollection<Order> Order { get; set; }
        public ICollection<WhishList> WhishList { get; set; }

        public override string ToString()
        {
            return $"Id: {Id}, UserName: {UserName}, UserEmail: {UserEmail}, IsStatus: {IsStatus}, Role: {Role},Password:{Password}";
        }
    }
}
