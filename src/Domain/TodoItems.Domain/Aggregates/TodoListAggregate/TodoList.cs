using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using TodoItems.Domain._Common;
using TodoItems.Domain._Common.Events;
using TodoItems.Domain._Common.Exceptions;
using TodoItems.Domain.Aggregates.TodoListAggregate.Interfaces;
using TodoItems.Domain.Aggregates.TodoListAggregate.ValeObjects;

namespace TodoItems.Domain.Aggregates.TodoListAggregate;

public class TodoList : EntityBaseGuid, ITodoList, IAggregateRoot
{
    public TodoList() { }
    private readonly ILogger<TodoList> _logger = NullLogger<TodoList>.Instance;

    #region DomainEvents

    private readonly List<INotification> _domainEvents = [];
    public IReadOnlyCollection<INotification> DomainEvents => _domainEvents.AsReadOnly();
    public void AddDomainEvent(INotification domainEvent) => _domainEvents.Add(domainEvent);
    public void RemoveDomainEvent(INotification domainEvent) => _domainEvents.Remove(domainEvent);
    public void ClearDomainEvents() => _domainEvents.Clear();

    #endregion DomainEvents

    private List<TodoItem> _items = [];
    public virtual IReadOnlyCollection<TodoItem> Items => _items.AsReadOnly();

    public void AddItem(int id, string title, string description, Category category)
    {
        if (_items.Exists(x => x.Id == id))
            throw new DomainValidationException($"El ítem con ID {id} ya existe en la lista.");

        var newItem = new TodoItem(id, title, description, category);

        _items.Add(newItem);

        AddDomainEvent(new TodoItemCreatedEvent(Id, id));

        _logger.LogInformation($"Ítem '{title}' añadido exitosamente.");
    }

    public void UpdateItem(int id, string description)
    {
        var item = _items.FirstOrDefault(x => x.Id == id);

        if (item is null)
            throw new DomainValidationException($"No se puede actualizar porque no se encontró el ítem con ID {id}.");

        //REQUERIDO: No se permitirá actualizar ni borrar un TodoItem que tenga más del 50% realizado.
        if (item.TotalProgress > 50)
            throw new DomainValidationException("No se puede actualizar el ítem porque tiene más del 50% realizado.");

        item.UpdateDescription(description);

        _logger.LogInformation($"Descripción del ítem {id} actualizada.");
    }

    public void RemoveItem(int id)
    {
        var item = _items.FirstOrDefault(x => x.Id == id);

        if (item is null)
            throw new DomainValidationException($"No se puede eliminar porque no se encontró el ítem con ID {id}.");

        //REQUERIDO: No se permitirá actualizar ni borrar un TodoItem que tenga más del 50% realizado.        
       if (item.TotalProgress > 50)
            throw new DomainValidationException("No se puede eliminar el ítem porque tiene más del 50% realizado.");

        if (!_items.Remove(item))
            throw new DomainValidationException($"No se puede eliminar el ítem con ID {id}.");

        _logger.LogInformation($"Ítem {id} eliminado.");
    }

    public void RegisterProgression(int id, DateTime dateTime, decimal percent)
    {
        var item = _items.FirstOrDefault(x => x.Id == id);

        if (item is null)
            throw new DomainValidationException($"No se puede registrar el progreso porque no se encontró el ítem con ID {id}.");

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

        _logger.LogInformation($"Progreso de {percent}% registrado para el ítem {id}.");
    }


    /*  REQUERIDO: 
     
        Cuando se ejecute el PrintItems, se mostrarán todos los TodoItems que tenga el agregado ordenado por el Id. 
        La cabecera de cada TodoItem tendrá el siguiente formato: {Id}) {Title} - {Description} ({Category}) Completed:{IsCompleted}.

        Ej: 1) Complete Project Report - Finish the final report for the project (Work) Completed:True
    */
    public void PrintItems()
    {
        Console.WriteLine("\n--- LISTADO DE TAREAS ---");

        foreach (var item in _items.OrderBy(x => x.Id))
        {
            Console.WriteLine(item.ToFullString());

            /*  REQUERIDO: 
             
                Para cada elemento dentro de la lista de Progression, se mostrará la fecha, el porcentaje acumulado hasta ese
                elemento y una barra de progreso que hará más visible el progreso realizado.
             

                Ej: 1) Complete Project Report - Finish the final report for the project (Work) Completed:True
                    3/18/2025 12:00:00 AM -  30% |OOOOOOOOOOOOOOO |
                    3/19/2025 12:00:00 AM -  80% |OOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOO |
                    3/20/2025 12:00:00 AM - 100% |OOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOO|
             */
            if (item.Progressions.Count > 0)
            {
                foreach (var progression in item.Progressions.OrderBy(x => x.Date))
                {
                    Console.WriteLine(progression.ToFullString());
                }
            }
        }

        Console.WriteLine("-------------------------\n");
    }
}
