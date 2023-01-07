using System.Net;
using IceVault.Application.SystemErrors.Entities;
using IceVault.WebApi.Test.Setup;
using Newtonsoft.Json;
using Shouldly;
using Xunit;

namespace IceVault.WebApi.Test.SystemErrors;

public class SystemErrorControllerTest : IntegrationTest
{
    public SystemErrorControllerTest(IceVaultWebApplicationFactory factory) 
        : base(factory)
    {
    }

    [Fact]
    public async Task GetSystemErrorsAsync_ShouldReturnAnEmptyListByDefault_Test()
    {
        var response = await GetAuthenticatedAsync("api/v1/system-errors");
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        var result = JsonConvert.DeserializeObject<List<SystemError>>(await response.Content.ReadAsStringAsync());
        result.ShouldBe(Array.Empty<SystemError>());
    }
}