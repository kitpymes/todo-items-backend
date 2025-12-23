using System.Globalization;

namespace TodoItems.Domain.ValueObjects;

public sealed class Progression : IEquatable<Progression>
{
    public DateTime Date { get; private set; }
    public decimal Percent { get; private set; }

    // Constructor requerido por EF Core
    private Progression() { }

    public Progression(DateTime date, decimal percent)
    {
        if (percent < 0 || percent > 100)
            throw new ArgumentException("Percent must be between 0 and 100");

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
        ReferenceEquals(left, right) || (left is not null && left.Equals(right));

    public static bool operator !=(Progression? left, Progression? right) => !(left == right);

    public override string ToString() => $"{Date:O} - {Percent.ToString("P", CultureInfo.InvariantCulture)}";    
}