using StackExchange.Redis;
using System.Text.Json;
using Talabat.Domain.Shared.Exceptions;
using Talabat.Domain.Models;
using Talabat.Domain.Services;

namespace Talabat.Infrastructure.Services
{
    internal class BasketService(IConnectionMultiplexer redis) : IBasketService
    {
        private readonly IDatabase _database = redis.GetDatabase();

        public async Task<CustomerBasket?> GetCustomerBasketAsync(string basketId)
        {
            var value = await _database.StringGetAsync(basketId);
            return value.IsNullOrEmpty ? throw new NotFoundException("No basket found!") : JsonSerializer.Deserialize<CustomerBasket>(json: value!)!;
        }

        public async Task CreateOrUpdateCustomerBasketAsync(CustomerBasket customerBasket)
        {
            var value = JsonSerializer.Serialize(customerBasket);
            await _database.StringSetAsync(customerBasket.Id, value, TimeSpan.FromDays(7));
            await GetCustomerBasketAsync(customerBasket.Id);
        }

        public async Task DeleteCustomerBasketAsync(string basketId)
        {
            var result = await _database.KeyDeleteAsync(basketId);
            if (!result) throw new NotFoundException("No basket found!");
        }
    }
}
