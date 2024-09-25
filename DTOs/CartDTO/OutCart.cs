namespace Ecommerce_Webapi.DTOs.CartDTO
{
    public class OutCart
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public string Img { get; set; }
        public decimal Total { get; set; }

    }
}
