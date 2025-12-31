using MediatR;
using TodoItems.Domain._Common;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using TodoItems.Domain._Common.Exceptions;
using TodoItems.Domain.Aggregates.TodoListAggregate.Entities;
using TodoItems.Domain.Aggregates.TodoListAggregate.ValeObjects;
using TodoItems.Domain.Aggregates.TodoListAggregate.Events;

namespace TodoItems.Domain.Aggregates.TodoListAggregate;

public class TodoList : AggregateRoot, ITodoList
{
    private readonly ILogger<TodoList> _logger = NullLogger<TodoList>.Instance;

    private List<TodoItem> _items = [];
    public virtual IReadOnlyCollection<TodoItem> Items => _items.AsReadOnly();

    public string Title { get; private set; } = string.Empty;

    public string? Description { get; private set; }

    private TodoList() { }

    public TodoList(string title, string? description = null)
    {
        UpdateTitle(title); 
        UpdateDescription(description);
    }

    public void UpdateTitle(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new DomainValidationException("El título es obligatorio.");

        if (title == Title)
            return;

        Title = title;

        _logger.LogInformation($"Título de la lista actualizado a '{Title}'.");
    }

    public void UpdateDescription(string? description)
    {
        if (string.IsNullOrWhiteSpace(description))
            return;

        if ( description == Description)
            return;

        Description = description;

        _logger.LogInformation("Descripción de la lista actualizada.");
    }

    public void AddItem(int id, string title, Category category, string? description = null)
    {
        if (_items.Exists(x => x.Id == id))
            throw new DomainValidationException($"El ítem con ID {id} ya existe en la lista.");

        var newItem = new TodoItem(id, title, category, description);

        _items.Add(newItem);

        AddDomainEvent(new TodoItemCreatedEvent(Id, id));

        _logger.LogInformation($"Ítem '{title}' añadido exitosamente.");
    }

    public void UpdateItemTitle(int id, string title)
    {
        var item = _items.FirstOrDefault(x => x.Id == id);

        if (item is null)
            throw new DomainValidationException($"No se puede actualizar porque no se encontró el ítem con ID {id}.");

        //REQUERIDO: No se permitirá actualizar ni borrar un TodoItem que tenga más del 50% realizado.
        if (item.TotalProgress > 50)
            throw new DomainValidationException("No se puede actualizar el ítem porque tiene más del 50% realizado.");

        item.UpdateTitle(title);

        _logger.LogInformation($"Título del ítem {id} actualizado.");
    }

    public void UpdateItemDescription(int id, string description)
    {
        var item = _items.FirstOrDefault(x => x.Id == id);

        if (item is null)
            throw new DomainValidationException($"No se puede actualizar porque no se encontró el ítem con ID {id}.");

        //REQUERIDO: No se permitirá actualizar ni borrar un TodoItem que tenga más del 50% realizado.
        if (item.TotalProgress > 50)
            throw new DomainValidationException("No se puede actualizar el ítem porque tiene más del 50% realizado.");

        item.UpdateDescription(description);

        _logger.LogInformation($"Descripción del ítem {id} actualizado.");
    }

    public void RemoveItem(int itemId)
    {
        var item = _items.FirstOrDefault(x => x.Id == itemId);

        if (item is null)
            throw new DomainValidationException($"No se puede eliminar porque no se encontró el ítem con ID {itemId}.");

        //REQUERIDO: No se permitirá actualizar ni borrar un TodoItem que tenga más del 50% realizado.        
       if (item.TotalProgress > 50)
            throw new DomainValidationException("No se puede eliminar el ítem porque tiene más del 50% realizado.");

        if (!_items.Remove(item))
            throw new DomainValidationException($"No se puede eliminar el ítem con ID {itemId}.");

        _logger.LogInformation($"Ítem {itemId} eliminado.");
    }

    public void RegisterItemProgression(int itemId, DateTime dateTime, decimal percent)
    {
        var item = _items.FirstOrDefault(x => x.Id == itemId);

        if (item is null)
            throw new DomainValidationException($"No se puede registrar el progreso porque no se encontró el ítem con ID {itemId}.");

        // REQUERIDO: se validará que la fecha de la nueva Progression sea mayor a las progresiones que ya tenga guardadas.
        if (item.Progressions.Any(x => x.Date >= dateTime))
            throw new DomainValidationException("No se puede registrar un progreso con una fecha anterior al último progreso registrado.");

        // REQUERIDO: se validará que el porcentaje sea válido (más grande de cero y menor a 100)
        if (percent < 0 || percent > 100)
            throw new DomainValidationException("No se puede registrar un progreso porque debe ser más grande de cero y menor a 100.");

        // REQUERIDO: una vez añadido a la lista con la Progression, no se sobrepase el 100% después de sumarlas todas.
        var totalPercent = item.Progressions.Sum(p => p.Percent) + percent;
        if (totalPercent > 100)
            throw new DomainValidationException("No se puede registrar un progreso porque la suma total de los porcentajes excede el 100%.");

        item.AddProgression(new Progression(dateTime, percent));

        _logger.LogInformation($"Progreso de {percent}% registrado para el ítem {itemId}.");
    }

    public void PrintItems()
    {
        Console.WriteLine($"\n --- Tareas del {Title}:  {Description}");

        foreach (var item in _items.OrderBy(x => x.Id))
        {
            Console.WriteLine();

            Console.WriteLine(item.ToFullString());

            if (item.Progressions.Count > 0)
            {
                var percentSum = 0m;

                foreach (var progression in item.Progressions.OrderBy(x => x.Date))
                {
                    percentSum += progression.Percent;

                    Console.WriteLine(progression.ToFullString(percentSum));
                }
            }
        }

        Console.WriteLine("\n-------------------------\n");
    }
}
