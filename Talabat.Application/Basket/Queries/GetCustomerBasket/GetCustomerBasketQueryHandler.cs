namespace Talabat.Application.Basket.Queries.GetCustomerBasket
{
    internal class GetCustomerBasketQueryHandler(
        IBasketService _basketService
        ) : IRequestHandler<GetCustomerBasketQuery, CustomerBasket>
    {
        public async Task<CustomerBasket> Handle(GetCustomerBasketQuery request, CancellationToken cancellationToken)
        {
            var basket = await _basketService.GetCustomerBasketAsync(request.BasketId);

            return basket!;
        }
    }
}
