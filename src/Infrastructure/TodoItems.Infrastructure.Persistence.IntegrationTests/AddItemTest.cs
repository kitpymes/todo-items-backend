//using FluentAssertions;
//using MediatR;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.DependencyInjection;
//using TodoItems.Application.Item.EventHandlers;
//using TodoItems.Application.Item.UseCases.Commands;
//using TodoItems.Application.Item.UseCases.Queries;
//using TodoItems.Domain._Common.Events;
//using TodoItems.Domain.Entities;
//using TodoItems.Infrastructure.IntegrationTests.Fakes;
//using TodoItems.Infrastructure.Persistence;

//namespace TodoItems.Infrastructure.IntegrationTests;

//public class AddItemTest
//{
//    [Fact]
//    public async Task SaveChangesAsync_Should_Publish_ItemCreatedEvent_Via_Mediator()
//    {
//        var services = new ServiceCollection();

//        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(ItemCreatedEventHandler).Assembly));

//        var received = new List<ItemCreatedEvent>();
//        services.AddSingleton<INotificationHandler<ItemCreatedEvent>>(new FakeNotificationEventHandle<ItemCreatedEvent>(received));

//        services.AddDbContext<ItemDbContext>(options =>
//            options.UseInMemoryDatabase(Guid.NewGuid().ToString()));

//        var provider = services.BuildServiceProvider();

//        using var scope = provider.CreateScope();
//        var context = scope.ServiceProvider.GetRequiredService<ItemDbContext>();

//        var item = new Item("Title", "Desc", "Cat");
//        context.Items.Add(item);

//        await context.SaveChangesAsync();

//        // Verificar que el handler fue invocado y recibió el id correcto
//        received.Should().ContainSingle().Which.Should().Be(item);

//        received.Should().ContainSingle(e => e is ItemCreatedEvent);
//    }

//    [Fact]
//    public async Task SavingItem_ShouldExecuteDomainEvent()
//    {
//        var services = new ServiceCollection();

//        services.AddMediatR(config => config.RegisterServicesFromAssemblies(AppDomain.CurrentDomain.GetAssemblies()));

//        //  services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(ItemCreatedEventHandler).Assembly));

//        services.AddDbContext<ItemDbContext>(options =>
//            options.UseInMemoryDatabase(Guid.NewGuid().ToString()));

//        var provider = services.BuildServiceProvider();

//        using var scope = provider.CreateScope();
//        var context = scope.ServiceProvider.GetRequiredService<ItemDbContext>();
        
//        var result = context.Items.Add(new Item("Title", "Desc", "Cat"));

//        await context.SaveChangesAsync();

//        var item = await context.Items.FirstAsync(i => i.Id == result.Entity.Id);

//        item.Should().NotBeNull();
//    }
//}