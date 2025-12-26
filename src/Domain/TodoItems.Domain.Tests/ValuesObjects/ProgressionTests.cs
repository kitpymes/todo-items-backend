using FluentAssertions;
using System.Globalization;
using TodoItems.Domain._Common.Exceptions;
using TodoItems.Domain.Aggregates.TodoListAggregate.ValeObjects;

namespace TodoItems.Domain.Tests.ValuesObjects;

public class ProgressionTests
{
    [Fact]
    public void Constructor_Should_SetProperties()
    {
        var date = new DateTime(2025, 1, 1, 12, 0, 0, DateTimeKind.Utc);
        var progression = new Progression(date, 42.5m);

        progression.Date.Should().Be(date);
        progression.Percent.Should().Be(42.5m);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(101)]
    public void Constructor_Should_Throw_WhenPercentOutOfRange(decimal invalidPercent)
    {
        Action act = () => new Progression(DateTime.UtcNow, invalidPercent);

        act.Should()
           .Throw<DomainValidationException>()
           .WithMessage("El Porcentaje debe estar entre 0 y 100.");
    }

    [Fact]
    public void Equals_Should_BeTrue_ForSameValues()
    {
        var date = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        var a = new Progression(date, 50m);
        var b = new Progression(date, 50m);

        a.Should().Be(b);
        (a == b).Should().BeTrue();
        (a != b).Should().BeFalse();
        a.GetHashCode().Should().Be(b.GetHashCode());
    }

    [Fact]
    public void ToString_Should_Return_O_Format_WithPercent()
    {
        var date = new DateTime(2025, 1, 1, 12, 0, 0, DateTimeKind.Utc);
        var percent = 12.34m;
        var progression = new Progression(date, 12.34m);

        progression.ToString().Should().Be($"{date:O} - {percent.ToString("P", CultureInfo.InvariantCulture)}");
    }
}