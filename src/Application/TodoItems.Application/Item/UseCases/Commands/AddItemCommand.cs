using MediatR;
using TodoItems.Domain._Common.AppResults;

namespace TodoItems.Application.Item.UseCases.Commands;

public record AddItemCommand(
    string Title,
    string Category,
    string Description
) : IRequest<IAppResult>;
