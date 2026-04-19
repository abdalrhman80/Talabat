using Talabat.Domain.Models;

namespace Talabat.Domain.Services
{
    public interface IBasketService
    {
        Task<CustomerBasket?> GetCustomerBasketAsync(string basketId);
        Task CreateOrUpdateCustomerBasketAsync(CustomerBasket customerBasket);
        Task DeleteCustomerBasketAsync(string basketId);
    }
}
