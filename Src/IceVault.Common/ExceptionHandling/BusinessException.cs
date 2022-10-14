namespace IceVault.Common.ExceptionHandling;

public class BusinessException : IceVaultException
{
    public BusinessException(Failure failure) 
        : base(failure)
    {
    }

    public BusinessException(List<Failure> failures) 
        : base(failures)
    {
    }
}