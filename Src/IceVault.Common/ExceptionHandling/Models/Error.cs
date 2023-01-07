namespace IceVault.Common.ExceptionHandling.Models;

public class Error
{
    public Error(int statusCode, IEnumerable<Failure> failures)
    {
        StatusCode = statusCode;
        Failures = failures;
    }
    
    public int StatusCode { get; }
    
    public IEnumerable<Failure> Failures { get; }
}