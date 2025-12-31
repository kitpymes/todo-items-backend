using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using TodoItems.Application.UseCases.TodoListUseCases.CreateTodoItem;
using TodoItems.Application.UseCases.TodoListUseCases.CreateTodoList;
using TodoItems.Application.UseCases.TodoListUseCases.GetCategories;
using TodoItems.Application.UseCases.TodoListUseCases.GetTodoItems;
using TodoItems.Application.UseCases.TodoListUseCases.GetTodoList;
using TodoItems.Application.UseCases.TodoListUseCases.RegisterProgressTodoItem;
using TodoItems.Application.UseCases.TodoListUseCases.RemoveTodoItem;
using TodoItems.Application.UseCases.TodoListUseCases.UpdateTodoItem;
using TodoItems.Application.UseCases.TodoListUseCases.UpdateTodoList;
using TodoItems.Domain._Common.AppResults;
using TodoItems.Presentation.API._Common;

namespace TodoItems.Presentation.API.Controllers;

[ApiVersion("1.0")]
public class TodoListController : ApiControllerBase
{
    /// <summary>
    /// Obtiene todas las listas de poryectos con sus tareas disponibles.
    /// </summary>
    /// <remarks>
    /// Este endpoint recupera una colección global de todos los proyectos de tareas (Todo Lists). 
    /// </remarks>
    /// <response code="200">Retorna la lista de colecciones encontrada exitosamente.</response>
    [HttpGet]
    public async Task<IAppResult> GetAllTodoList()
        => await Mediator.Send(new GetTodoListQuery());

    /// <summary>
    /// Crea una nuevo proyecto.
    /// </summary>
    /// <remarks>
    /// El cuerpo de la petición debe incluir obligatoriamente un título.
    /// </remarks>
    /// <response code="200">Proyecto creado exitosamente.</response>
    /// <response code="400">Datos de entrada inválidos.</response>
    [HttpPost]
    [ProducesResponseType(typeof(IAppResultError), StatusCodes.Status400BadRequest)]
    public async Task<IAppResult> CreateTodoList([FromBody] CreateTodoListRequest request)
        => await Mediator.Send(new CreateTodoListCommand(request.Title, request.Description));

    /// <summary>
    /// Actualiza un proyecto existente.
    /// </summary>
    /// <param name="todoListId">ID del proyecto.</param>
    /// <param name="request">Contenido actualizado.</param>
    /// <response code="200">Cambios guardados correctamente.</response>
    /// <response code="400">Los datos del request son incorrectos.</response>
    [HttpPut("{todoListId}")]
    [ProducesResponseType(typeof(IAppResultError), StatusCodes.Status400BadRequest)]
    public async Task<IAppResult> UpdateTodoList(Guid todoListId, [FromBody] UpdateTodoListRequest request)
        => await Mediator.Send(new UpdateTodoListCommand(todoListId, request.Title, request.Description));

    /// <summary>
    /// Lista todas las categorías disponibles para clasificar tareas.
    /// </summary>
    /// <remarks>
    /// Útil para poblar componentes desplegables (dropdowns) al crear o editar una tarea.
    /// </remarks>
    /// <response code="200">Retorna el listado de categorías.</response>
    [HttpGet("categories")]
    public async Task<IAppResult> GetAllCategories()
        => await Mediator.Send(new GetCategoriesQuery());

    /// <summary>
    /// Recupera todos los ítems pertenecientes a una proyecto específico.
    /// </summary>
    /// <param name="todoListId">El identificador único (GUID) del proyecto.</param>
    /// <remarks>
    /// Utilice este endpoint cuando el usuario seleccione una proyecto específico para ver sus tareas pendientes y completadas.
    /// </remarks>
    /// <response code="200">Retorna los ítems vinculados al ID del proyecto proporcionado.</response>
    /// <response code="400">Si el formato del GUID es inválido o la lista no existe.</response>
    [HttpGet("{todoListId}/items")]
    [ProducesResponseType(typeof(IAppResultError), StatusCodes.Status400BadRequest)]
    public async Task<IAppResult> GetAllTodoItems(Guid todoListId)
        => await Mediator.Send(new GetTodoItemsByTodoListIdQuery(todoListId));

    /// <summary>
    /// Crea una nueva tarea dentro de un proyecto específico.
    /// </summary>
    /// <param name="todoListId">ID del proyecto donde se guardara el ítem.</param>
    /// <param name="request">Objeto con el título, descripción y categoría de la nueva tarea.</param>
    /// <remarks>
    /// El cuerpo de la petición debe incluir obligatoriamente un título. La categoría debe coincidir con un ID de categoría válido.
    /// </remarks>
    /// <response code="200">Tarea creada exitosamente.</response>
    /// <response code="400">Datos de entrada inválidos o IDs no encontrados.</response>
    [HttpPost("{todoListId}/item")]
    [ProducesResponseType(typeof(IAppResultError), StatusCodes.Status400BadRequest)]
    public async Task<IAppResult> CreateTodoItem(Guid todoListId, [FromBody] CreateTodoItemRequest request)
        => await Mediator.Send(new CreateTodoItemCommand(todoListId, request.Title, request.Category, request.Description));

    /// <summary>
    /// Actualiza una tarea existente.
    /// </summary>
    /// <param name="todoListId">ID del proyecto.</param>
    /// <param name="itemId">ID numérico de la tarea a modificar.</param>
    /// <param name="request">Contenido actualizado.</param>
    /// <response code="200">Cambios guardados correctamente.</response>
    /// <response code="400">El ítem no pertenece a la lista indicada o los datos son incorrectos.</response>
    [HttpPut("{todoListId}/item/{itemId}")]
    [ProducesResponseType(typeof(IAppResultError), StatusCodes.Status400BadRequest)]
    public async Task<IAppResult> UpdateTodoItem(Guid todoListId, int itemId, [FromBody] UpdateTodoItemRequest request)
        => await Mediator.Send(new UpdateTodoItemCommand(todoListId, itemId, request.Title, request.Description));

    /// <summary>
    /// Elimina de forma permanente una tarea.
    /// </summary>
    /// <param name="todoListId">ID de la lista contenedora.</param>
    /// <param name="itemId">ID del ítem a eliminar.</param>
    /// <response code="200">Ítem borrado con éxito.</response>
    /// <response code="400">No se pudo encontrar el ítem para eliminar.</response>
    [HttpDelete("{todoListId}/item/{itemId}")]
    [ProducesResponseType(typeof(IAppResultError), StatusCodes.Status400BadRequest)]
    public async Task<IAppResult> RemoveTodoItem(Guid todoListId, int itemId)
        => await Mediator.Send(new RemoveTodoItemCommand(todoListId, itemId));

    /// <summary>
    /// Registra el porcentaje de avance de una tarea específica.
    /// </summary>
    /// <param name="todoListId">ID de la lista contenedora.</param>
    /// <param name="itemId">ID de la tarea.</param>
    /// <param name="request">Porcentaje de progreso (0 a 100).</param>
    /// <remarks>
    /// Permite el seguimiento granular de tareas complejas. Si el porcentaje llega a 100, la tarea se considera finalizada.
    /// </remarks>
    /// <response code="200">Progreso actualizado correctamente.</response>
    /// <response code="400">El valor del porcentaje está fuera del rango permitido.</response>
    [HttpPost("{todoListId}/item/{itemId}/progression")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(IAppResultError), StatusCodes.Status400BadRequest)]
    public async Task<IAppResult> RegisterProgressTodoItem(Guid todoListId, int itemId, [FromBody] RegisterProgressTodoItemRequest request)
        => await Mediator.Send(new RegisterProgressTodoItemCommand(todoListId, itemId, request.Date, request.Percent));
}
