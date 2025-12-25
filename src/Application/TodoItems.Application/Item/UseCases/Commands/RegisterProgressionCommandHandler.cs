using MediatR;
using System.Net;
using TodoItems.Domain._Common.AppResults;
using TodoItems.Domain._Common.Interfaces;

namespace TodoItems.Application.Item.UseCases.Commands;

public class RegisterProgressionCommandHandler(IItemRepository repository) : IRequestHandler<RegisterProgressionCommand, IAppResult>
{
    private readonly IItemRepository _repository = repository;

    public async Task<IAppResult> Handle(RegisterProgressionCommand request, CancellationToken cancellationToken)
    {
        var item = await _repository.GetByIdAsync(request.ItemId, cancellationToken);

        if (item is null)
        {
            return AppResult.BadRequest($"El Item con el Id {request.ItemId} no fue existe.");
        }

        item.RegisterProgression(request.Percent);

        return AppResult.Success(HttpStatusCode.Created);
    }
}
