using AutoMapper;
using MediatR;
using TodoItems.Domain._Common.AppResults;
using TodoItems.Domain._Common.Interfaces;

namespace TodoItems.Application.Item.UseCases.Commands;

public class AddItemCommandHandler(IItemRepository repository, IMapper mapper) : IRequestHandler<AddItemCommand, IAppResult>
{
    private readonly IItemRepository _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task<IAppResult> Handle(AddItemCommand request, CancellationToken cancellationToken)
    {
        var item = _mapper.Map<Domain.Entities.Item>(request);

        await _repository.AddAsync(item, cancellationToken);

        return AppResult.Success(item);
    }
}