using FluentValidation;
using IWantApp.Domain.Products;

namespace IWantApp.Endpoints.Validators;

public class CategoryValidator : AbstractValidator<Category>
{
    public CategoryValidator()
    {
        RuleFor(c => c.Name)
            .NotNull()
            .NotEmpty()
            .MinimumLength(3);

        RuleFor(c => c.Active)
            .NotNull();

        RuleFor(c => c.CreatedBy)
            .NotNull()
            .NotEmpty();

        RuleFor(c => c.EditedBy)
            .NotNull()
            .NotEmpty();
    }
}
