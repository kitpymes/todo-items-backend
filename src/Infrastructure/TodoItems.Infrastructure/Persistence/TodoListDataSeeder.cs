using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoItems.Domain.Aggregates.TodoListAggregate;
using TodoItems.Domain.Aggregates.TodoListAggregate.ValeObjects;

namespace TodoItems.Infrastructure.Persistence;

public static class TodoDataSeeder
{
    public static async Task SeedAsync(TodoListDbContext context)
    {
        // 1. Verificar si ya existen datos para no duplicar
        if (await context.TodoLists.AnyAsync()) return;

        // 2. Crear la Raíz del Agregado
        var projectReport = new TodoList();

        // 3. Definir categorías usando nuestro Value Object
        var workCat = new Category("Work");
        var managementCat = new Category("Management");
        var researchCat = new Category("Research");

        // 4. Agregar ítems a través de la raíz (esto disparará los eventos de dominio)
        projectReport.AddItem(101, "Configuración de Arquitectura", "Configurar el esqueleto de DDD y EF Core", workCat);
        projectReport.AddItem(102, "Reunión de Stakeholders", "Presentación de avances del primer sprint", managementCat);
        projectReport.AddItem(103, "Investigación de UI/UX", "Analizar tendencias de diseño para 2026", researchCat);

        // 5. Opcional: Registrar algunos progresos iniciales manualmente
        // Aunque el ItemCreatedEventHandler lo hará al 0%, aquí podemos poner avances reales
        projectReport.RegisterProgression(101, DateTime.UtcNow.AddDays(-1), 10);
        projectReport.RegisterProgression(101, DateTime.UtcNow, 25);

        // 6. Guardar en la base de datos
        context.TodoLists.Add(projectReport);

        await context.SaveChangesAsync();
    }
}