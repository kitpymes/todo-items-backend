using Microsoft.AspNetCore.Mvc;
using TodoItems.Application.TodoList.DTOs;
using TodoItems.Application.TodoList.UseCases.Commands;
using TodoItems.Application.TodoList.UseCases.Queries;
using TodoItems.Domain._Common.AppResults;

namespace TodoItems.Presentation.API.Controllers;

public class TodoListController : ApiControllerBase
{
    [HttpGet]
    [Produces("application/json")]
    [ProducesResponseType(typeof(IAppResultSuccess), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(IAppResultError), StatusCodes.Status500InternalServerError)]
    public async Task<IAppResult> GetAllTodoList()
        => await Mediator.Send(new GetTodoListQuery());

    [HttpGet("{todoListId}/items")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(IAppResultSuccess), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(IAppResultError), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(IAppResultError), StatusCodes.Status500InternalServerError)]
    public async Task<IAppResult> GetAllItems(Guid todoListId)
        => await Mediator.Send(new GetItemsByTodoListIdQuery(todoListId));

    [HttpGet("categories")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(IAppResultSuccess), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(IAppResultError), StatusCodes.Status500InternalServerError)]
    public async Task<IAppResult> GetAllCategories()
        => await Mediator.Send(new GetCategoriesQuery());

    [HttpPost("{todoListId}/items")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(IAppResultSuccess), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(IAppResultError), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(IAppResultError), StatusCodes.Status500InternalServerError)]
    public async Task<IAppResult> AddItem(Guid todoListId, [FromBody] AddItemRequest request)
        => await Mediator.Send(new AddItemCommand(todoListId, request.Title, request.Description, request.Category));

    [HttpPut("{todoListId}/items/{itemId}")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(IAppResultSuccess), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(IAppResultError), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(IAppResultError), StatusCodes.Status500InternalServerError)]
    public async Task<IAppResult> UpdateItem(Guid todoListId, int itemId, [FromBody] UpdateItemRequest request)
        => await Mediator.Send(new UpdateItemCommand(todoListId, itemId, request.Description));

    [HttpDelete("{todoListId}/items/{itemId}")]
    [ProducesResponseType(typeof(IAppResultSuccess), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(IAppResultError), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(IAppResultError), StatusCodes.Status500InternalServerError)]
    public async Task<IAppResult> RemoveItem(Guid todoListId, int itemId)
        => await Mediator.Send(new RemoveItemCommand(todoListId, itemId));

    [HttpPost("{todoListId}/items/{itemId}/progression")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(IAppResultSuccess), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(IAppResultError), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(IAppResultError), StatusCodes.Status500InternalServerError)]
    public async Task<IAppResult> RegisterProgressItem(Guid todoListId, int itemId, [FromBody] RegisterProgressItemRequest request)
        => await Mediator.Send(new RegisterProgressItemCommand(todoListId, itemId, request.Percent));
}
