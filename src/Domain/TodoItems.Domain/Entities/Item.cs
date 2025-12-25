using TodoItems.Domain._Common;
using TodoItems.Domain._Common.Events;
using TodoItems.Domain._Common.Exceptions;

namespace TodoItems.Domain.Entities;

public class Item : EntityBaseGuid
{
    private readonly List<Progression> _progressions = [];
    public IReadOnlyCollection<Progression> Progressions => _progressions.AsReadOnly();

    public string Title { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public string Category { get; private set; } = string.Empty;

    private Item(): base() { }

    public Item(string title, string description, string category) : this()
    {
        if(string.IsNullOrWhiteSpace(title))
        {
            throw new DomainValidationException("El título es obligatorio.");
        }

        if (string.IsNullOrWhiteSpace(category))
        {
            throw new DomainValidationException("La Categoria es obligatorio.");
        }

        Title = title;
        Description = description;
        Category = category;

        AddDomainEvent(new ItemCreatedEvent(Id));
    }

    public void UpdateDescription(string description) => Description = description;

    public void RegisterProgression(decimal percent) => _progressions.Add(new Progression(DateTime.UtcNow, percent));
}
