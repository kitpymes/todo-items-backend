namespace TodoItems.Application.TodoList.DTOs;

public record AddItemRequest(string Title, string Description, string Category);