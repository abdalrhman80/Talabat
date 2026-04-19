using Talabat.Domain.Models;
using Talabat.Domain.Shared.Constants;

namespace Talabat.Domain.Services
{
    public interface IOrderService
    {
        Task UpdateOrderStatusAsync(Order order, OrderStatus orderStatus);
        Task UpdateOrderStatusTransactionAsync(Order order, OrderStatus orderStatus);
    }
}
