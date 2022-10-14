namespace IceVault.Common.ExceptionHandling;

public abstract class IceVaultException : ApplicationException
{
    private readonly List<Failure> _failures = new();

    protected IceVaultException(Failure failure)
        : this(new List<Failure>() { failure })
    {
    }

    protected IceVaultException(List<Failure> failures)
    {
        _failures.AddRange(failures);
    }

    public IReadOnlyList<Failure> Failures => _failures;
}