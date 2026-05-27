namespace TrackCare.Domain.Common;

public abstract class ValueObject
{
    public abstract IEnumerable<object?> GetAtomicValues();
    public abstract override bool Equals(object? obj);
    public abstract override int GetHashCode();

    protected static bool EqualOperator(ValueObject? left, ValueObject? right)
    {
        if (left is null ^ right is null)
            return false;
        return left is null || left.Equals(right);
    }

    protected static bool NotEqualOperator(ValueObject? left, ValueObject? right) => !EqualOperator(left, right);
}
