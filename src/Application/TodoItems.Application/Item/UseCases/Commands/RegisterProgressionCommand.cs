using MediatR;
using TodoItems.Domain._Common.AppResults;

namespace TodoItems.Application.Item.UseCases.Commands;

public record RegisterProgressionCommand(
    Guid ItemId,
    decimal Percent
) : IRequest<IAppResult>;