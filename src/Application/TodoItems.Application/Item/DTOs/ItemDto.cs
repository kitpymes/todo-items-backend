namespace TodoItems.Application.Item.DTOs;

public class ItemDto
{
    public Guid Id { get; init; }
    public string Title { get; init; } = default!;
    public string Description { get; init; } = default!;
    public string Category { get; init; } = default!;
}
