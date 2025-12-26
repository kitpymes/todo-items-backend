using FluentValidation;

namespace TodoItems.Application.TodoList.UseCases.Commands;

public class AddItemCommandValidator : AbstractValidator<AddItemCommand>
{
    public AddItemCommandValidator()
    {
        RuleFor(x => x.TodoListId)
            .NotEmpty().WithMessage("El Id de la lista de tareas es obligatorio.");

        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("El título es obligatorio.");

        RuleFor(x => x.Category)
           .NotEmpty().WithMessage("La Categoria es obligatorio.");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("La descripción es obligatoria.");
    }
}
