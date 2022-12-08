using System.Net;
using System.Text;
using IceVault.Presentation.Authentication.Models.Requests;
using IceVault.WebApi.Test.Setup;
using Newtonsoft.Json;
using Shouldly;
using Xunit;

namespace IceVault.WebApi.Test.Authentication;

public class AuthenticationControllerTest : IClassFixture<IceVaultWebApplicationFactory>
{
    private readonly HttpClient _client;
    
    public AuthenticationControllerTest(IceVaultWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }
    
    [Fact]
    public async Task LoginAsync_ShouldReturnJwtToken_WhenCredentialsAreValid_Test()
    {
        var request = new LoginRequest { Email = "test01@hotmail.com", Password = "Test01" };
        var json = JsonConvert.SerializeObject(request);

        var response = await _client.PostAsync("api/v1/auth/login", new StringContent(json, Encoding.UTF8, "application/json"));
        
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
    }
}