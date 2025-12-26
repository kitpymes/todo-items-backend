using System.Globalization;
using TodoItems.Domain._Common.Exceptions;

namespace TodoItems.Domain.Aggregates.TodoListAggregate.ValeObjects;

// REQUERIDO: Cada elemento Progression tendrá dos campos, la fecha en la que se ha realizado la acción y el porcentaje realizado.
public sealed class Progression : IEquatable<Progression>
{
    public DateTime Date { get; private set; }
    public decimal Percent { get; private set; }

    private Progression() { }

    public Progression(DateTime date, decimal percent)
    {
        if (percent < 0 || percent > 100)
            throw new DomainValidationException("El Porcentaje debe estar entre 0 y 100.");

        Date = date;
        Percent = percent;
    }

    public bool Equals(Progression? other)
    {
        if (ReferenceEquals(this, other)) return true;
        if (other is null) return false;
        return Date == other.Date && Percent == other.Percent;
    }

    public override bool Equals(object? obj) => Equals(obj as Progression);

    public override int GetHashCode() => HashCode.Combine(Date, Percent);

    public static bool operator ==(Progression? left, Progression? right) =>
        ReferenceEquals(left, right) || left is not null && left.Equals(right);

    public static bool operator !=(Progression? left, Progression? right) => !(left == right);

    public override string ToString() => $"{Date:O} - {Percent.ToString("P", CultureInfo.InvariantCulture)}";

    public string ToFullString() 
    {
        var progressBarLength = 50;
        var filledLength = (int)(Percent / 100 * progressBarLength);
        var emptyLength = progressBarLength - filledLength;
        var progressBar = "|" + new string('O', filledLength) + new string(' ', emptyLength) + "|";

        return $"  {Date:MM/dd/yyyy} - {Percent,3}% {progressBar}";
    }
}