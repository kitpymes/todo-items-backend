using TodoItems.Domain._Common;
using TodoItems.Domain._Common.Exceptions;
using TodoItems.Domain.Aggregates.TodoListAggregate.ValeObjects;

namespace TodoItems.Domain.Aggregates.TodoListAggregate.Entities;

public class TodoItem : EntityBaseInt
{
    private readonly List<Progression> _progressions = [];
    public IReadOnlyCollection<Progression> Progressions => _progressions.AsReadOnly();

    public string Title { get; private set; } = string.Empty;
    
    public Category Category { get; private set; } = null!;

    public string? Description { get; private set; }

    //private TodoItem() { }

    public TodoItem(int id, string title, Category category, string? description = null) : base(id)
    {
        if(id <= 0)
            throw new DomainValidationException("El Id debe ser un número positivo mayor que cero.");

        UpdateTitle(title);
        UpdateDescription(description);

        Category = category;
    }

    internal void UpdateTitle(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new DomainValidationException("El título es obligatorio.");

        if (title == Title)
            return;

        Title = title;
    }

    internal void UpdateDescription(string? description)
    {
        if (string.IsNullOrWhiteSpace(description))
            return;

        if (description == Description)
            return;

        Description = description;
    }

    internal void AddProgression(Progression progression) => _progressions.Add(progression);

    public decimal TotalProgress => _progressions.Sum(p => p.Percent);

    public bool IsCompleted => _progressions.Any() && _progressions.Sum(p => p.Percent) >= 100;

    public string ToFullString()
        => $"{Id}) {Title} - {Description} ({Category.Name}) Completed:{IsCompleted}";
}
