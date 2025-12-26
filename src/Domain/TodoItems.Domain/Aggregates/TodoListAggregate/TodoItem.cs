using TodoItems.Domain._Common;
using TodoItems.Domain._Common.Events;
using TodoItems.Domain._Common.Exceptions;
using TodoItems.Domain.Aggregates.TodoListAggregate.ValeObjects;

namespace TodoItems.Domain.Aggregates.TodoListAggregate;

public class TodoItem : EntityBaseInt
{
    private readonly List<Progression> _progressions = [];
    public IReadOnlyCollection<Progression> Progressions => _progressions.AsReadOnly();

    public string Title { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public Category Category { get; private set; } = null!;

    private TodoItem() { }

    public TodoItem(int id, string title, string description, Category category) : base(id)
    {
        if(id <= 0)
            throw new DomainValidationException("El Id debe ser un número positivo mayor que cero.");

        if (string.IsNullOrWhiteSpace(title))
            throw new DomainValidationException("El título es obligatorio.");

        if (string.IsNullOrWhiteSpace(description))
            throw new DomainValidationException("La descripción es obligatoria.");

        if (category is null)
            throw new DomainValidationException("La categoria es obligatoria.");

        Title = title;
        Description = description;
        Category = category;
    }

    public void UpdateDescription(string description) => Description = description;

    public void AddProgression(Progression progression) => _progressions.Add(progression);

    public decimal TotalProgress => _progressions.Sum(p => p.Percent);

    public bool IsCompleted => _progressions.Any() && _progressions.Max(p => p.Percent) >= 100;

    public string ToFullString()
        => $"{Id}) {Title} - {Description} ({Category.Name}) Completed:{IsCompleted})";
}
