using FluentValidation;
using IWantApp.Domain.Products;

namespace IWantApp.Endpoints.Validators;

public class ProductValidator : AbstractValidator<Product>
{
    public ProductValidator()
    {
        RuleFor(p => p.Description)
            .NotEmpty()
            .NotNull()
            .MinimumLength(3);

        RuleFor(p => p.Name)
            .NotEmpty()
            .NotNull()
            .MinimumLength(3);

        RuleFor(p => p.Active)
            .NotNull();

        RuleFor(p => p.HasStock)
            .NotNull();

        RuleFor(p => p.CategoryId)
            .NotNull();
    }
}
