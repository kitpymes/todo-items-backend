using TodoItems.Application.Item.UseCases;
using TodoItems.Infrastructure.Persistence.Repositories;

var repository = new InMemoryItemRepository();

var addItem = new AddItemUseCase(repository);
var updateItem = new UpdateItemUseCase(repository);
var registerProgression = new RegisterProgressionUseCase(repository);
var printItems = new PrintItemsUseCase(repository);
var removeItem = new RemoveItemUseCase(repository);

// Add
await addItem.Execute("DDD Course", "Learn DDD properly", "Education");

// Update
await updateItem.Execute(new Guid(), "Learn DDD with real examples");

// Progress
await registerProgression.Execute(Guid.NewGuid(), DateTime.Now, 25);
await registerProgression.Execute(Guid.NewGuid(), DateTime.Now.AddDays(1), 50);

// Print
await printItems.Execute();

// Remove
await removeItem.Execute(Guid.NewGuid());
