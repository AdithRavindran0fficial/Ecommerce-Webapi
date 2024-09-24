﻿using System.ComponentModel.DataAnnotations;

namespace Ecommerce_Webapi.DTOs.ProductDTO
{
    public class ProductViewDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        
        public string Description { get; set; }
        
        public string Img { get; set; }
        
        public decimal Price { get; set; }

        public string  Category { get; set; }

    }
}
