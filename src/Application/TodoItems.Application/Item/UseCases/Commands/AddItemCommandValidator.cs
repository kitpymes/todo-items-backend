using FluentValidation;

namespace TodoItems.Application.Item.UseCases.Commands;

public class AddItemCommandValidator : AbstractValidator<AddItemCommand>
{
    public AddItemCommandValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("El título es obligatorio.");

        RuleFor(x => x.Category)
           .NotEmpty().WithMessage("La Categoria es obligatorio.");
    }
}
