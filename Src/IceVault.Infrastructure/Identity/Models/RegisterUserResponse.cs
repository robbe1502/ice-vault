using Newtonsoft.Json;

namespace IceVault.Infrastructure.Identity.Models;

public class RegisterUserResponse
{
    [JsonProperty("id")]
    public Guid Id { get; set; }
}