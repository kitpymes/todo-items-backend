using System.Globalization;
using TodoItems.Domain._Common.Exceptions;

namespace TodoItems.Domain.Entities;

public sealed class Progression : IEquatable<Progression>
{
    public DateTime Date { get; private set; }
    public decimal Percent { get; private set; }

    private Progression() { }

    public Progression(DateTime date, decimal percent)
    {
        if(date.Kind != DateTimeKind.Utc)
        {
            throw new DomainValidationException("La fecha debe estar en formato UTC.");
        }

        if (percent < 0 || percent > 100)
        {
            throw new DomainValidationException("El Porcentaje debe estar entre 0 y 100.");
        }

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
}