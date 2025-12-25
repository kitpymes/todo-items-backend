using Microsoft.AspNetCore.Mvc;
using TodoItems.Application.Item.UseCases.Commands;
using TodoItems.Application.Item.UseCases.Queries;
using TodoItems.Domain._Common.AppResults;

namespace TodoItems.Presentation.API.Controllers;

public class ItemController : ApiControllerBase
{
    [HttpGet]
    [Produces("application/json")]
    [ProducesResponseType(typeof(IAppResultSuccess), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(IAppResultError), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(IAppResultError), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(IAppResultError), StatusCodes.Status500InternalServerError)]
    public async Task<IAppResult> GetAll() => await Mediator.Send(new GetItemsQuery());

    [HttpPost]
    [Produces("application/json")]
    [ProducesResponseType(typeof(IAppResultSuccess), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(IAppResultError), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(IAppResultError), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(IAppResultError), StatusCodes.Status500InternalServerError)]
    public async Task<IAppResultBase> Add(AddItemCommand request) => await Mediator.Send(request);

    [HttpPost]
    [Produces("application/json")]
    [ProducesResponseType(typeof(IAppResultSuccess), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(IAppResultError), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(IAppResultError), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(IAppResultError), StatusCodes.Status500InternalServerError)]
    public async Task<IAppResult> RegisterProgress(RegisterProgressionCommand request) => await Mediator.Send(request);
}
