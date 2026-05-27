using FluentValidation.Results;

namespace TrackCare.Application.Common.Exceptions;

public class ValidationException : Exception
{
    public ValidationException(IEnumerable<ValidationFailure> failures)
        : base(string.Join("; ", failures.Select(f => $"{f.PropertyName}: {f.ErrorMessage}")))
    {
        Failures = failures.ToList();
    }

    public IReadOnlyList<ValidationFailure> Failures { get; }
}
