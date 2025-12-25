using AutoMapper;
using MediatR;
using TodoItems.Application.Item.DTOs;
using TodoItems.Domain._Common.AppResults;
using TodoItems.Domain._Common.Interfaces;

namespace TodoItems.Application.Item.UseCases.Queries;

public class GetItemsQueryHandler(IItemRepository repository, IMapper mapper) : IRequestHandler<GetItemsQuery, IAppResult>
{
    private readonly IItemRepository _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task<IAppResult> Handle(GetItemsQuery request, CancellationToken cancellationToken)
    {
        var items = await _repository.GetAllAsync(cancellationToken);

        var dtos = _mapper.Map<IReadOnlyCollection<ItemDto>>(items);

        return AppResult.Success(dtos);
    }
}
