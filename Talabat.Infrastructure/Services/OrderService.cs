using Talabat.Domain.Models;
using Talabat.Domain.Repositories;
using Talabat.Domain.Services;
using Talabat.Domain.Shared.Constants;

namespace Talabat.Infrastructure.Services
{
    internal class OrderService(
        IUnitOfWork _unitOfWork
        ) : IOrderService
    {
        public async Task UpdateOrderStatusTransactionAsync(Order order, OrderStatus orderStatus)
        {
            await _unitOfWork.BeginTransactionAsync();

            try
            {
                await UpdateOrderStatusAsync(order, orderStatus);

                await _unitOfWork.CompleteAsync();
                await _unitOfWork.CommitTransactionAsync();
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task UpdateOrderStatusAsync(Order order, OrderStatus orderStatus)
        {
            order.Status = orderStatus;
            order.UpdatedAt = DateTime.Now.ToLocalTime();

            var orderProductsIds = order.OrderItems.Select(oi => oi.Product.Id).ToList();
            var updatedProducts = new List<Product>();

            if (orderStatus == OrderStatus.Cancelled || orderStatus == OrderStatus.Refunded)
            {
                foreach (var productId in orderProductsIds)
                {
                    var product = await _unitOfWork.Repository<Product>().GetByIdAsync(productId)!;
                    product!.StockQuantity += order.OrderItems.First(oi => oi.Product.Id == productId).Quantity;
                    updatedProducts.Add(product);
                }
            }

            _unitOfWork.Repository<Order>().Update(order);
            _unitOfWork.Repository<Product>().UpdateRange(updatedProducts);
        }

    }
}
