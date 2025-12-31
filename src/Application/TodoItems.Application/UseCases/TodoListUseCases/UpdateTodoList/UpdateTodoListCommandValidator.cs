using FluentValidation;

namespace TodoItems.Application.UseCases.TodoListUseCases.UpdateTodoList;

public class UpdateTodoListCommandValidator : AbstractValidator<UpdateTodoListCommand>
{
    public UpdateTodoListCommandValidator()
    {
        RuleFor(x => x.TodoListId).NotEmpty().WithMessage("El Id del proyecto es obligatorio.");

        RuleFor(x => x.Title).NotEmpty().WithMessage("La título es obligatorio.");
    }
}
