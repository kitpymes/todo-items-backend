using FluentAssertions;
using TodoItems.Domain._Common.Exceptions;
using TodoItems.Domain.Aggregates.TodoListAggregate;
using TodoItems.Domain.Aggregates.TodoListAggregate.ValeObjects;

namespace TodoItems.Domain.Tests;

public class ItemTests
{
    [Fact]
    public void CreateItem_ShouldCreateSuccessfully()
    {
        var id = new Random().Next();
        var title = Guid.NewGuid().ToString();
        var description = Guid.NewGuid().ToString();
        var categoryName = Guid.NewGuid().ToString();
        var category = new Category(categoryName);

        var item = new TodoItem(id, title, description, category);

        item.Id.Should().Be(id);
        item.Title.Should().Be(title);
        item.Description.Should().Be(description);
        item.Category.Should().Be(category);
        item.Category.Name.Should().Be(categoryName);
    }

    [Fact]
    public void CreateItem_WithoutTitle_ShouldThrow()
    {
        Action act = () => new TodoItem(1, "", "Desc", new Category(Guid.NewGuid().ToString()));

        act.Should().Throw<DomainValidationException>()
           .WithMessage("El título es obligatorio.");
    }

    [Fact]
    public void UpdateDescription_ShouldChangeDescription()
    {
        var item = new TodoItem(1, "Title", "Old", new Category(Guid.NewGuid().ToString()));

        item.UpdateDescription("New");

        item.Description.Should().Be("New");
    }

    [Fact]
    public void RegisterProgression_ShouldAddProgression()
    {
        var item = new TodoItem(1, "", "Desc", new Category(Guid.NewGuid().ToString()));
        var progession = new Progression(DateTime.UtcNow, 30);

        item.AddProgression(progession);

        item.Progressions.Should().HaveCount(1);
        item.Progressions.Should().Contain(progession);
        item.TotalProgress.Should().Be(30);
        item.IsCompleted.Should().BeFalse();
    }

    [Fact]
    public void RegisterProgression_WithInvalidPercent_ShouldThrow()
    {
        var item = new TodoItem(1, "", "Desc", new Category(Guid.NewGuid().ToString()));
        var progession = new Progression(DateTime.UtcNow, 130);

        Action act = () => item.AddProgression(progession);

        act.Should().Throw<DomainValidationException>();
    }
}