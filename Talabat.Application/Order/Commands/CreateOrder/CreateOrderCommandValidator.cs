namespace Talabat.Application.Order.Commands.CreateOrder
{
    public class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
    {
        public CreateOrderCommandValidator()
        {
            RuleFor(o => o.BasketId)
                .NotEmpty().WithMessage("Basket Id is required");

            RuleFor(x => x.PaymentMethodId)
                .GreaterThan(0).WithMessage("Payment method must be selected.");

            RuleFor(x => x.DeliveryMethodId)
                .GreaterThan(0).WithMessage("Delivery method must be selected.");

            RuleFor(x => x.ShippingAddress)
                .NotNull().WithMessage("Shipping address is required.");

            When(x => x.ShippingAddress != null, () =>
            {
                RuleFor(x => x.ShippingAddress.Street)
                    .NotEmpty().WithMessage("Street is required.")
                    .MaximumLength(200);

                RuleFor(x => x.ShippingAddress.City)
                    .NotEmpty().WithMessage("City is required.")
                    .MaximumLength(100);

                RuleFor(x => x.ShippingAddress.State)
                    .NotEmpty().WithMessage("State is required.")
                    .MaximumLength(100);

                RuleFor(x => x.ShippingAddress.Country)
                    .NotEmpty().WithMessage("Country is required.")
                    .MaximumLength(100);
            });
        }
    }
}
