using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using TodoItems.Application.Item.Mappings;
using TodoItems.Application.Item.UseCases.Commands;
using TodoItems.Application.Item.UseCases.Queries;
using TodoItems.Domain._Common.Interfaces;
using TodoItems.Infrastructure.Extensions;
using TodoItems.Infrastructure.Middlewares;
using TodoItems.Infrastructure.Persistence;
using TodoItems.Infrastructure.Persistence.Repositories;

var builder = WebApplication.CreateBuilder(args); 

builder.Services.AddControllers(); 

builder.Services.AddEndpointsApiExplorer();

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    options.SerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase;
    options.SerializerOptions.PropertyNameCaseInsensitive = true;
});

builder.Services.AddSwaggerGen();

builder.Services.AddAutoMapper(typeof(ItemMapperProfile).Assembly);

builder.Services.AddFluentValidation(typeof(AddItemCommandValidator).Assembly);

builder.Services.AddMediatR(config => {
    config.RegisterServicesFromAssembly(typeof(GetItemsQueryHandler).Assembly);
    //config.AddOpenBehavior(typeof(ValidationBehavior<,>));
});

builder.Services.AddScoped<IItemRepository, ItemRepository>();

builder.Services.AddDbContext<ItemDbContext>(options => options.UseInMemoryDatabase("ItemsDb"));

var app = builder.Build(); 
if (app.Environment.IsDevelopment()) { app.UseSwagger(); app.UseSwaggerUI(); }
app.UseMiddleware<AppValidationsMiddleware>();
app.MapControllers(); 
app.Run();

public partial class Program { }