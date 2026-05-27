namespace TrackCare.Application.Common.Models;

public class Result<T>
{
    internal Result(bool succeeded, T? data, string[] errors)
    {
        Succeeded = succeeded;
        Data = data;
        Errors = errors;
    }

    public bool Succeeded { get; init; }
    public T? Data { get; init; }
    public string[] Errors { get; init; }

    public static Result<T> Success(T data) => new(true, data, []);
    public static Result<T> Failure(string[] errors) => new(false, default, errors);
}
