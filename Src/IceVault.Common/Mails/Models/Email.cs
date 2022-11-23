namespace IceVault.Common.Mails.Models;

public class Email
{
    private readonly List<string> _receivers = new List<string>();

    public Email(string subject, string body)
    {
        Subject = subject;
        Body = body;
    }
    
    public string Subject { get; }
    
    public string Body { get; }

    public void AddReceiver(string receiver)
    {
        _receivers.Add(receiver);
    }

    public IReadOnlyCollection<string> Receivers => _receivers;
}