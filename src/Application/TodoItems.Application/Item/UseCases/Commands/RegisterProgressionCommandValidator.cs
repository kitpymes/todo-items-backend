using FluentValidation;

namespace TodoItems.Application.Item.UseCases.Commands;

public class RegisterProgressionCommandValidator : AbstractValidator<RegisterProgressionCommand>
{
    public RegisterProgressionCommandValidator()
    {
        RuleFor(x => x.ItemId)
            .NotEmpty().WithMessage("El Id del Item es obligatorio.");

        RuleFor(p => p.Percent)
            .GreaterThan(0)
            .LessThanOrEqualTo(100)
            .WithMessage("El Porcentaje debe estar entre 0 y 100.");
    }
}