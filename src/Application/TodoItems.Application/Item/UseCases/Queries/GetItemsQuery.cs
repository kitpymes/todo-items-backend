using MediatR;
using TodoItems.Domain._Common.AppResults;

namespace TodoItems.Application.Item.UseCases.Queries;

public record GetItemsQuery() : IRequest<IAppResult>;
