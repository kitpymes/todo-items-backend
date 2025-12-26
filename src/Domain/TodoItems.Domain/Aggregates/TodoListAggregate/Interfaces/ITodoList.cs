using TodoItems.Domain.Aggregates.TodoListAggregate.ValeObjects;

namespace TodoItems.Domain.Aggregates.TodoListAggregate.Interfaces;


/* REQUERIDO: crear una aplicación que gestione una lista de TodoItems. Esta lista debe estar contenida dentro de un agregado que
cumpla exactamente esta interfaz.
   
    public interface ITodoList
    {
        void AddItem(int id, string title, string description, string category);
        void UpdateItem(int id, string description);
        void RemoveItem(int id);
        void RegisterProgression(int id, DateTime dateTime, decimal percent);
        void PrintItems();
    }
 
 */
public interface ITodoList
{
    void AddItem(int id, string title, string description, Category category);
    void UpdateItem(int id, string description);
    void RemoveItem(int id);
    void RegisterProgression(int id, DateTime dateTime, decimal percent);
    void PrintItems();
}
