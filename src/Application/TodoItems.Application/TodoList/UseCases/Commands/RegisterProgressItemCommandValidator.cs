using FluentValidation;

namespace TodoItems.Application.TodoList.UseCases.Commands;

public class RegisterProgressItemCommandValidator : AbstractValidator<RegisterProgressItemCommand>
{
    public RegisterProgressItemCommandValidator()
    {
        RuleFor(x => x.TodoListId)
            .NotEmpty().WithMessage("El Id de la lista de tareas es obligatorio.");

        RuleFor(x => x.ItemId)
            .NotEmpty().WithMessage("El Id del Item es obligatorio.");

        RuleFor(p => p.Percent)
            .GreaterThan(0)
            .LessThanOrEqualTo(100)
            .WithMessage("El Porcentaje debe estar entre 0 y 100.");
    }
}