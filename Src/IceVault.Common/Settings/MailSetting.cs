namespace IceVault.Common.Settings;

public class MailSetting
{
    public string Host { get; set; }

    public int Port { get; set; }
    
    public bool IsSslEnabled { get; set; }
    
    public string UserName { get; set; }
    
    public string Password { get; set; }
    
    public string FromEmailAddress { get; set; }

    public bool IsInvalid => string.IsNullOrWhiteSpace(Host);
}