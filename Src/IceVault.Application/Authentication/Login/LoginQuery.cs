using IceVault.Common.Messaging;

namespace IceVault.Application.Authentication.Login;

public sealed class LoginQuery : IQuery<LoginResult>
{
    public LoginQuery(string email, string password)
    {
        Email = email;
        Password = password;
    }

    public string Email { get; }

    public string Password { get; }
}