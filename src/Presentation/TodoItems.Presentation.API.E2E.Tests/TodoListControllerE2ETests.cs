using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Net.Http.Json;
using TodoItems.Application.TodoList.DTOs;
using TodoItems.Domain.Aggregates.TodoListAggregate;
using TodoItems.Domain.Aggregates.TodoListAggregate.ValeObjects;
using TodoItems.Infrastructure.Persistence;

namespace TodoItems.Presentation.API.E2E.Tests;

public class TodoListControllerE2ETests(CustomWebApplicationFactory factory) : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client = factory.CreateClient();

    [Fact]
    public async Task GetAllCategories_ShouldReturnOk()
    {
        // Arrange
        var requestUri = "api/v1/todo-list/categories";

        // Act
        var response = await _client.GetAsync(requestUri);
        var result = await response.Content.ToAppResult<List<string>>();   
        response.EnsureSuccessStatusCode();

        // Assert
        Assert.NotNull(response);
        Assert.NotNull(result);
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Data);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.True(response.IsSuccessStatusCode, $"Esperaba: {HttpStatusCode.OK} | Devolvió: {response.StatusCode}");
    }

    [Fact]
    public async Task GetAllTodoList_ShouldReturnOk()
    {
        // Arrange
        var requestUri = "api/v1/todo-list";

        // Act
        var response = await _client.GetAsync(requestUri);        
        var result =  await response.Content.ToAppResult<List<TodoList>>();
        response.EnsureSuccessStatusCode();

        // Assert
        Assert.NotNull(response);
        Assert.NotNull(result);
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Data);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.True(response.IsSuccessStatusCode, $"Esperaba: {HttpStatusCode.OK} | Devolvió: {response.StatusCode}");
    }

    #region GetItem

    [Fact]
    public async Task GetAllItems_ShouldReturnOk()
    {
        // Arrange
        using var scope = factory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<TodoListDbContext>();

        var todoList = new TodoList();
        var category = new Category(Guid.NewGuid().ToString());

        todoList.AddItem(new Random().Next(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), category);
        todoList.AddItem(new Random().Next(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), category);
        todoList.AddItem(new Random().Next(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), category);

        context.TodoLists.Add(todoList);

        await context.SaveChangesAsync();

        var requestUri = $"api/v1/todo-list/{todoList.Id}/items";

        // Act
        var response = await _client.GetAsync(requestUri);
        var result = await response.Content.ToAppResult<List<TodoItemReportDto>>();
        response.EnsureSuccessStatusCode();

        // Assert
        Assert.NotNull(response);
        Assert.NotNull(result);
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Data);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.True(response.IsSuccessStatusCode, $"Esperaba: {HttpStatusCode.OK} | Devolvió: {response.StatusCode}");
    }

    [Fact]
    public async Task GetAllItems_InvalidTodoListId_ShouldReturnBadRequest()
    {
        // Arrange
        var todoListId = Guid.NewGuid();
        var requestUri = $"api/v1/todo-list/{todoListId}/items";

        // Act
        var response = await _client.GetAsync(requestUri);
        var result = await response.Content.ToAppResult<List<TodoItemReportDto>>();

        // Assert
        Assert.NotNull(response);
        Assert.NotNull(result);
        Assert.False(result.IsSuccess);
        Assert.NotEmpty(result.Message!);
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    #endregion GetItem

    #region AddItem

    [Fact]
    public async Task AddItem_ShouldCreateItem()
    {
        // Arrange
        using var scope = factory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<TodoListDbContext>();

        var todoList = new TodoList();
        var category = new Category(Guid.NewGuid().ToString());

        todoList.AddItem(new Random().Next(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), category);
        todoList.AddItem(new Random().Next(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), category);
        todoList.AddItem(new Random().Next(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), category);

        context.TodoLists.Add(todoList);

        await context.SaveChangesAsync();

        var payload = new
        {
            Title = Guid.NewGuid().ToString(),
            Description = Guid.NewGuid().ToString(),
            Category = category.Name
        };

        var requestUri = $"api/v1/todo-list/{todoList.Id}/item";

        // Act
        var response = await _client.PostAsJsonAsync(requestUri, payload);
        var result = await response.Content.ToAppResult<int>();
        response.EnsureSuccessStatusCode();

        // Assert
        Assert.NotNull(response);
        Assert.NotNull(result);
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Data);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.True(response.IsSuccessStatusCode, $"Esperaba: {HttpStatusCode.OK} | Devolvió: {response.StatusCode}");
    }

    [Fact]
    public async Task AddItem_InvalidTodoListId_ShouldReturnBadRequest()
    {
        // Arrange
        var todoListId = Guid.NewGuid();
        var payload = new
        {
            Title = Guid.NewGuid().ToString(),
            Description = Guid.NewGuid().ToString(),
            Category = Guid.NewGuid().ToString()
        };

        var requestUri = $"api/v1/todo-list/{todoListId}/item";

        // Act
        var response = await _client.PostAsJsonAsync(requestUri, payload);
        var result = await response.Content.ToAppResult<int>();

        // Assert
        Assert.NotNull(response);
        Assert.NotNull(result);
        Assert.False(result.IsSuccess);
        Assert.NotEmpty(result.Message!);
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);    
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public async Task AddItem_InvalidModel_TitleEmpty_ShouldReturnBadRequest(string? title)
    {
        // Arrange
        using var scope = factory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<TodoListDbContext>();

        var todoList = new TodoList();
        var category = new Category(Guid.NewGuid().ToString());

        todoList.AddItem(new Random().Next(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), category);
        todoList.AddItem(new Random().Next(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), category);
        todoList.AddItem(new Random().Next(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), category);

        context.TodoLists.Add(todoList);

        await context.SaveChangesAsync();

        var payload = new
        {
            Title = title,
            Description = Guid.NewGuid().ToString(),
            Category = category.Name
        };

        var requestUri = $"api/v1/todo-list/{todoList.Id}/item";

        // Act
        var response = await _client.PostAsJsonAsync(requestUri, payload);
        var result = await response.Content.ToAppResult<int>();

        // Assert
        Assert.NotNull(response);
        Assert.NotNull(result);
        Assert.False(result.IsSuccess);
        Assert.NotEmpty(result.Message!);
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public async Task AddItem_InvalidModel_DescriptionEmpty_ShouldReturnBadRequest(string? description)
    {
        // Arrange
        using var scope = factory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<TodoListDbContext>();

        var todoList = new TodoList();
        var category = new Category(Guid.NewGuid().ToString());

        todoList.AddItem(new Random().Next(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), category);
        todoList.AddItem(new Random().Next(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), category);
        todoList.AddItem(new Random().Next(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), category);

        context.TodoLists.Add(todoList);

        await context.SaveChangesAsync();

        var payload = new
        {
            Title = Guid.NewGuid().ToString(),
            Description = description,
            Category = category.Name
        };

        var requestUri = $"api/v1/todo-list/{todoList.Id}/item";

        // Act
        var response = await _client.PostAsJsonAsync(requestUri, payload);
        var result = await response.Content.ToAppResult<int>();

        // Assert
        Assert.NotNull(response);
        Assert.NotNull(result);
        Assert.False(result.IsSuccess);
        Assert.NotEmpty(result.Message!);
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public async Task AddItem_InvalidModel_CategoryEmpty_ShouldReturnBadRequest(string? category)
    {
        // Arrange
        using var scope = factory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<TodoListDbContext>();

        var todoList = new TodoList();
        var newCategory = new Category(Guid.NewGuid().ToString());

        todoList.AddItem(new Random().Next(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), newCategory);
        todoList.AddItem(new Random().Next(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), newCategory);
        todoList.AddItem(new Random().Next(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), newCategory);

        context.TodoLists.Add(todoList);

        await context.SaveChangesAsync();

        var payload = new
        {
            Title = Guid.NewGuid().ToString(),
            Description = Guid.NewGuid().ToString(),
            Category = category
        };

        var requestUri = $"api/v1/todo-list/{todoList.Id}/item";

        // Act
        var response = await _client.PostAsJsonAsync(requestUri, payload);
        var result = await response.Content.ToAppResult<int>();

        // Assert
        Assert.NotNull(response);
        Assert.NotNull(result);
        Assert.False(result.IsSuccess);
        Assert.NotEmpty(result.Message!);
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    #endregion AddItem

    #region UpdateItem

    [Fact]
    public async Task UpdateItem_ShouldModifyExistingItem()
    {
        // Arrange
        using var scope = factory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<TodoListDbContext>();

        var todoList = new TodoList();
        var category = new Category(Guid.NewGuid().ToString());
        var itemId = new Random().Next();

        todoList.AddItem(itemId, Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), category);
        context.TodoLists.Add(todoList);
        await context.SaveChangesAsync();

        var payload = new
        {
            Description = "Nueva Descripción Actualizada"
        };

        var requestUri = $"api/v1/todo-list/{todoList.Id}/item/{itemId}";

        // Act
        var response = await _client.PutAsJsonAsync(requestUri, payload);
        var result = await response.Content.ToAppResult();
        response.EnsureSuccessStatusCode();

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        using var scope1 = factory.Services.CreateScope();
        var context1 = scope1.ServiceProvider.GetRequiredService<TodoListDbContext>();
        var itemInDb = context1.TodoLists.SelectMany(l => l.Items).FirstOrDefault(i => i.Id == itemId);

        Assert.NotNull(itemInDb);
        Assert.Equal("Nueva Descripción Actualizada", itemInDb.Description);
    }

    [Fact]
    public async Task UpdateItem_InvalidTodoListId_ShouldReturnBadRequest()
    {
        // Arrange
        using var scope = factory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<TodoListDbContext>();

        var todoList = new TodoList();
        var category = new Category(Guid.NewGuid().ToString());
        var itemId = new Random().Next();

        todoList.AddItem(itemId, Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), category);
        context.TodoLists.Add(todoList);
        await context.SaveChangesAsync();

        var payload = new
        {
            Description = "Nueva Descripción Actualizada"
        };

        var requestUri = $"api/v1/todo-list/{Guid.NewGuid()}/item/{itemId}";

        // Act
        var response = await _client.PutAsJsonAsync(requestUri, payload);
        var result = await response.Content.ToAppResult();

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)] 
    public async Task UpdateItem_InvalidModel_ShouldReturnBadRequest(string? description)
    {
        // Arrange
        using var scope = factory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<TodoListDbContext>();
        var todoList = new TodoList();
        var category = new Category(Guid.NewGuid().ToString());
        var itemId = new Random().Next();
        todoList.AddItem(itemId, Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), category);
        context.TodoLists.Add(todoList);
        await context.SaveChangesAsync();

        var payload = new
        {
            Description = description
        };
        var requestUri = $"api/v1/todo-list/{todoList.Id}/item/{itemId}";

        // Act
        var response = await _client.PutAsJsonAsync(requestUri, payload);
        var result = await response.Content.ToAppResult();

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    #endregion

    #region DeleteItem

    [Fact]
    public async Task RemoveItem_ShouldDeleteFromTodoList()
    {
        // Arrange
        using var scope = factory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<TodoListDbContext>();

        var todoList = new TodoList();
        var category = new Category(Guid.NewGuid().ToString());
        var itemId = new Random().Next();

        todoList.AddItem(itemId, Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), category);
        context.TodoLists.Add(todoList);
        await context.SaveChangesAsync();

        var requestUri = $"api/v1/todo-list/{todoList.Id}/item/{itemId}";

        // Act
        var response = await _client.DeleteAsync(requestUri);
        var result = await response.Content.ToAppResult();

        // Assert
        response.EnsureSuccessStatusCode();
        Assert.True(result.IsSuccess);

        using var scope1 = factory.Services.CreateScope();
        var context1 = scope1.ServiceProvider.GetRequiredService<TodoListDbContext>();
        var exists = context1.TodoLists.SelectMany(l => l.Items).Any(i => i.Id == itemId);

        Assert.False(exists, "El ítem aún persiste en la base de datos.");
    }

    #endregion DeleteItem

    #region RegisterProgression

    [Fact]
    public async Task RegisterProgression_ShouldRecordProgress()
    {
        // Arrange
        using var scope = factory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<TodoListDbContext>();

        var todoList = new TodoList();
        var category = new Category(Guid.NewGuid().ToString());
        var itemId = new Random().Next();

        todoList.AddItem(itemId, Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), category);
        context.TodoLists.Add(todoList);
        await context.SaveChangesAsync();

        var payload = new
        {
            Percent = 50        
        };

        var requestUri = $"api/v1/todo-list/{todoList.Id}/item/{itemId}/progression";

        // Act
        var response = await _client.PostAsJsonAsync(requestUri, payload);
        var result = await response.Content.ToAppResult();

        // Assert
        response.EnsureSuccessStatusCode();
        Assert.True(result.IsSuccess);

        using var scope1 = factory.Services.CreateScope();
        var context1 = scope1.ServiceProvider.GetRequiredService<TodoListDbContext>();
        var itemInDb = context1.TodoLists
            .SelectMany(l => l.Items)
            .Include(i => i.Progressions)
            .FirstOrDefault(i => i.Id == itemId);

        Assert.NotNull(itemInDb);
        Assert.Contains(itemInDb.Progressions, p => p.Percent == 50);
    }

    #endregion RegisterProgression
}
