namespace Talabat.Application.Basket.Command.DeleteCustomerBasket
{
    public record DeleteCustomerBasketCommand(string BasketId) : IRequest;
}
