namespace Talabat.Application.Basket.Command.DeleteCustomerBasket
{
    internal class DeleteCustomerBasketCommandHandler(
        IBasketService _basketService
        ) : IRequestHandler<DeleteCustomerBasketCommand>
    {
        public async Task Handle(DeleteCustomerBasketCommand request, CancellationToken cancellationToken)
        {
            await _basketService.DeleteCustomerBasketAsync(request.BasketId);
        }
    }
}
