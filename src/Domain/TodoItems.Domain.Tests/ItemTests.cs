using FluentAssertions;
using TodoItems.Domain._Common.Exceptions;
using TodoItems.Domain.Entities;
namespace TodoItems.Domain.Tests;

public class ItemTests
{
    [Fact]
    public void CreateItem_ShouldCreateSuccessfully()
    {
        var item = new Item("Title", "Description", "Category");

        item.Id.Should().Be(item.Id);
        item.Title.Should().Be("Title");
        item.Description.Should().Be("Description");
        item.Category.Should().Be("Category");
    }

    [Fact]
    public void CreateItem_WithoutTitle_ShouldThrow()
    {
        Action act = () => new Item("", "Desc", "Cat");

        act.Should().Throw<DomainValidationException>()
           .WithMessage("El título es obligatorio.");
    }

    [Fact]
    public void UpdateDescription_ShouldChangeDescription()
    {
        var item = new Item("Title", "Old", "Cat");

        item.UpdateDescription("New");

        item.Description.Should().Be("New");
    }

    [Fact]
    public void RegisterProgression_ShouldAddProgression()
    {
        var item = new Item("Title", "Desc", "Cat");

        item.RegisterProgression(50);

        item.Progressions.Should().HaveCount(1);
    }

    [Fact]
    public void RegisterProgression_WithInvalidPercent_ShouldThrow()
    {
        var item = new Item("Title", "Desc", "Cat");

        Action act = () => item.RegisterProgression(150);

        act.Should().Throw<DomainValidationException>();
    }
}