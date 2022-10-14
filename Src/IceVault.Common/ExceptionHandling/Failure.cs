namespace IceVault.Common.ExceptionHandling;

public class Failure
{
    public Failure(string code, string message)
    {
        Code = code;
        Message = message;
    }

    public string Code { get; }

    public string Message { get; }
}