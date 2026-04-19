namespace Talabat.Application.Basket.Queries.GetCustomerBasket
{
    public record GetCustomerBasketQuery(string BasketId) : IRequest<CustomerBasket>;
}
