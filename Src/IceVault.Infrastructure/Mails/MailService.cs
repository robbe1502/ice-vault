using System.Net;
using System.Net.Mail;
using IceVault.Common.ExceptionHandling;
using IceVault.Common.Mails;
using IceVault.Common.Settings;
using Microsoft.Extensions.Options;

namespace IceVault.Infrastructure.Mails;

internal class MailService : IMailService
{
    private readonly MailSetting _settings;
    
    public MailService(IOptions<MailSetting> options)
    {
        _settings = options.Value;
    }
    
    public async Task Send(IEmailMaker maker)
    {
        if (_settings.IsInvalid) throw new BusinessException(FailureConstant.MailService.InvalidConfiguration);

        var client = new SmtpClient(_settings.Host)
        {
            Port = _settings.Port,
            Credentials = new NetworkCredential(_settings.UserName, _settings.UserName),
            EnableSsl = _settings.IsSslEnabled
        };

        var email = await maker.Make();

        var message = new MailMessage()
        {
            From = new MailAddress(_settings.FromEmailAddress),
            Subject = email.Subject,
            Body = email.Body,
            IsBodyHtml = true,
        };

        var receivers = email.Receivers.Select(el => new MailAddress(el));
        foreach (var receiver in receivers)
        {
            message.To.Add(receiver);
        }

        await client.SendMailAsync(message);
    }
}