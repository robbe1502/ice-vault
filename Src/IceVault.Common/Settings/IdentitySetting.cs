namespace IceVault.Common.Settings;

public class IdentitySetting
{
    public string Authority { get; set; }

    public string ClientId { get; set; }

    public string ClientSecret { get; set; }

    public string Scope { get; set; }

    public bool RequireHttps { get; set; }
}