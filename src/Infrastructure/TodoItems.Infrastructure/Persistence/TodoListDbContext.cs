using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TodoItems.Domain.Aggregates.TodoListAggregate;
using TodoItems.Infrastructure.Extensions;

namespace TodoItems.Infrastructure.Persistence;

public class TodoListDbContext(DbContextOptions<TodoListDbContext> options, IServiceProvider serviceProvider) : DbContext(options)
{
    private readonly IMediator _mediator = serviceProvider.GetRequiredService<IMediator>();

    public DbSet<TodoList> TodoLists { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(TodoListDbContext).Assembly);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var result = await base.SaveChangesAsync(cancellationToken);

        await _mediator.DispatchDomainEventsAsync(this);

        return result;
    }
}
