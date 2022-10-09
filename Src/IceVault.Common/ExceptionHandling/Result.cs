using System.Runtime.InteropServices;

namespace IceVault.Common.ExceptionHandling;

public class Result
{
    private readonly List<Failure> _failures = new();

    protected internal Result(bool isSuccess, Failure failure) 
        : this(isSuccess, new List<Failure>() { failure })
    {
    }

    protected internal Result(bool isSuccess, List<Failure> failures)
    {
        IsSuccess = isSuccess;
        _failures.AddRange(failures);
    }

    public bool IsSuccess { get; }

    public bool IsFailure => !IsSuccess;

    public IReadOnlyCollection<Failure> Failures => _failures;

    public static Result Success() => new(true, new List<Failure>());

    public static Result<T> Success<T>(T value) => new(value, true, new List<Failure>());

    public static Result Failure(Failure failure) => new(false, failure);

    public static Result Failure(List<Failure> failures) => new(false, failures);
}

public class Result<T> : Result
{
    private readonly T _value;

    protected internal Result(T value, bool isSuccess, List<Failure> failures) 
        : base(isSuccess, failures)
    {
        _value = value;
    }

    public T Value => IsSuccess ? _value : throw new InvalidOperationException("Value of failure result can not be accessed.");
}