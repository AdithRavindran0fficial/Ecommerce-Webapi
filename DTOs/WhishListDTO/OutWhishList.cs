namespace Ecommerce_Webapi.DTOs.WhishListDTO
{
    public class OutWhishList
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Img { get; set; }
        public decimal price { get; set; }
        public string category { get; set; }
    }
}
