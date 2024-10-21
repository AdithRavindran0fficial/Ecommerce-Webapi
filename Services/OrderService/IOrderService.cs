using Ecommerce_Webapi.DTOs.OrderDTO;

namespace Ecommerce_Webapi.Services.OrderService
{
    public interface IOrderService
    {
        Task<string> OrderCreate(long price);
        bool Payment(PaymentDto razorpay);
        Task<bool> OrderPlace(int id, OrderDTO orderDTO);
        Task<IEnumerable<OutOrders>> GetOrderDetail(int id);
        Task<List<OutOrders>> GetAllOrdersAdmin(int id);
        Task<decimal> TotalRevenue();
        Task<int> TotalProductPurchased();
        
    }
}
