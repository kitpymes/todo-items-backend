using Microsoft.EntityFrameworkCore;
using TodoItems.Domain.Aggregates.TodoListAggregate;
using TodoItems.Domain.Aggregates.TodoListAggregate.ValeObjects;

namespace TodoItems.Infrastructure.Persistence;

public static class TodoListDataSeeder
{
    public static void Seed(TodoListDbContext context)
    {
        if (context.TodoLists.Any()) return;

        var projectReport = new TodoList("Proyecto Test", "Proyecto creado con Seed Data utilizando First Code");

        var cat1 = new Category("Normal");
        var cat2 = new Category("Urgente");
        var cat3 = new Category("Baja");

        projectReport.AddItem(101, "Configuración de Arquitectura", cat1, "Configurar el esqueleto de DDD y EF Core");
        projectReport.AddItem(102, "Reunión de Stakeholders", cat2, "Presentación de avances del primer sprint");
        projectReport.AddItem(103, "Investigación de UI/UX", cat3, "Analizar tendencias de diseño para 2026");

        context.TodoLists.Add(projectReport);

        context.SaveChanges();

        var todoList = context.TodoLists
            .Include(t => t.Items)            
                .ThenInclude(i => i.Progressions)
            .FirstOrDefault();

        todoList.RegisterItemProgression(101, DateTime.UtcNow.AddHours(1), 30);
        todoList.RegisterItemProgression(101, DateTime.UtcNow.AddHours(2), 50);
        todoList.RegisterItemProgression(101, DateTime.UtcNow.AddHours(3), 20);

        context.TodoLists.Update(todoList);

        context.SaveChanges();
    }
}