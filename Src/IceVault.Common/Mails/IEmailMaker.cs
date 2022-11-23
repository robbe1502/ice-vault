using IceVault.Common.Mails.Models;

namespace IceVault.Common.Mails;

public interface IEmailMaker
{
    Task<Email> Make();
}