namespace IceVault.Common.ExceptionHandling;

public class Failure
{
    private Failure(string code, string message)
    {
        Code = code;
        Message = message;
    }

    public string Code { get; }
    public string Message { get; }

    public static Failure Create(string code, string message)
    {
        return new Failure(code, message);
    }
}