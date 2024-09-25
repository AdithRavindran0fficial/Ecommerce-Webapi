namespace Ecommerce_Webapi.DTOs.CartItemDTO
{
    public class CartItemDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Img { get; set; }
        public decimal Price { get; set; }
        public int quantity { get; set; }

        public decimal PriceTotal { get; set; }
    }
}
