using IceVault.Common.Mails;
using IceVault.Common.Mails.Models;

namespace IceVault.Infrastructure.Mails.Makers;

public class RegisterEmailMaker : IEmailMaker
{
    private readonly string _receiver;

    public RegisterEmailMaker(string receiver)
    {
        _receiver = receiver ?? throw new ArgumentNullException(nameof(receiver));
    }
    
    public Task<Email> Make()
    {
        var email = new Email("Welcome To IceVault", "A test body");
        email.AddReceiver(_receiver);
        
        return Task.FromResult(email);
    }
}