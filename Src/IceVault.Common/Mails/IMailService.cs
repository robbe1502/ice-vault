namespace IceVault.Common.Mails;

public interface IMailService
{
    Task Send(IEmailMaker maker);
}