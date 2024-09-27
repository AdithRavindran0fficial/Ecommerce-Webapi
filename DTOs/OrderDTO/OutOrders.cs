namespace Ecommerce_Webapi.DTOs.OrderDTO
{
    public class OutOrders
    {
        public int Id { get; set; }
        public int Product_Id { get; set; }
        public int Qty { get; set; }
        //public decimal Price { get; set; }
        public string Product_Name { get; set; }
        public decimal Total { get; set; }
        //public int User_Id { get; set; }
        //public string Img { get; set; }


    }
}
