using System.Text;
using IceVault.Application.Authentication.Login;
using IceVault.Common;
using IceVault.Persistence.Read;
using IceVault.Persistence.Write;
using IceVault.Presentation.Authentication.Models.Requests;
using IdentityModel.Client;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Xunit;

namespace IceVault.WebApi.Test.Setup;

public abstract class IntegrationTest : IClassFixture<IceVaultWebApplicationFactory>
{
    private readonly HttpClient _client;
    
    protected IntegrationTest(IceVaultWebApplicationFactory factory)
    {
        _client = factory.CreateDefaultClient();
        
        ReadDbContext = factory.Services.GetRequiredService<IceVaultReadDbContext>();
        WriteDbContext = factory.Services.GetRequiredService<IceVaultWriteDbContext>();
    }

    protected async Task<HttpResponseMessage> PostAsync(string url, string json, string correlationId = "")
    {
        _client.DefaultRequestHeaders.Add(IceVaultConstant.Tracing.CorrelationHeaderName, correlationId);
        return await _client.PostAsync(url, new StringContent(json, Encoding.UTF8, "application/json"));
    }

    protected async Task<HttpResponseMessage> PostAuthenticatedAsync(string url, string json)
    {
        await Authenticate();
        return await PostAsync(url, json);
    }

    protected async Task<HttpResponseMessage> GetAuthenticatedAsync(string url)
    {
        await Authenticate();
        return await _client.GetAsync(url);
    }
    
    private async Task Authenticate()
    {
        var request = new LoginRequest { Email = "test01@hotmail.com", Password = "Test01" };
        var response = await PostAsync("api/v1/auth/login", JsonConvert.SerializeObject(request));

        var body = JsonConvert.DeserializeObject<LoginResult>(await response.Content.ReadAsStringAsync());
        if (body == null) throw new ApplicationException("Could not authenticate user during integration testing");
        
        _client.SetBearerToken(body.AccessToken);
    }
    
    protected IceVaultWriteDbContext WriteDbContext { get; }
    
    protected IceVaultReadDbContext ReadDbContext { get; }
}