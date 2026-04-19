using static Talabat.Domain.Models.CustomerBasket;

namespace Talabat.Application.Basket.Command.CreateCustomerBasket
{
    public class CreateOrUpdateCustomerBasketCommand : IRequest
    {
        public string Id { get; set; } = default!;
        public ICollection<BasketItem> BasketItems { get; set; } = [];
    }
}
