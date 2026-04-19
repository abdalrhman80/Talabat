namespace Talabat.Application.Basket.Command.CreateCustomerBasket
{
    internal class CreateOrUpdateCustomerBasketCommandHandler(
        IBasketService _basketService
        ) : IRequestHandler<CreateOrUpdateCustomerBasketCommand>
    {
        public async Task Handle(CreateOrUpdateCustomerBasketCommand request, CancellationToken cancellationToken)
        {
            await _basketService.CreateOrUpdateCustomerBasketAsync(new CustomerBasket { Id = request.Id, BasketItems = request.BasketItems });
        }
    }
}
