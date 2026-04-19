namespace Talabat.Application.Products.Commands.CreateProduct
{
    public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
    {
        public CreateProductCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Product name is required.")
                .MaximumLength(100)
                .WithMessage("Product name must not exceed 100 characters.");

            RuleFor(x => x.Description)
                .NotEmpty()
                .WithMessage("Product description is required.")
                .MaximumLength(500)
                .WithMessage("Product description must not exceed 500 characters.");

            RuleFor(x => x.Price)
                .GreaterThan(0)
                .WithMessage("Product price must be greater than zero.");

            RuleFor(x => x.StockQuantity)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Product quantity must be zero or greater.");
        }
    }
}
