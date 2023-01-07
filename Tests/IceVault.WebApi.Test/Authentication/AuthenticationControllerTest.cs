using System.Net;
using IceVault.Application.Authentication.Profile;
using IceVault.Common.Events.Authentication;
using IceVault.Common.ExceptionHandling;
using IceVault.Presentation.Authentication.Models.Demands;
using IceVault.Presentation.Authentication.Models.Requests;
using IceVault.WebApi.Test.Setup;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Shouldly;
using Xunit;

namespace IceVault.WebApi.Test.Authentication;

public class AuthenticationControllerTest : IntegrationTest
{
    public AuthenticationControllerTest(IceVaultWebApplicationFactory factory) 
        : base(factory)
    {
    }
    
    [Fact]
    public async Task LoginAsync_ShouldReturnJwtToken_WhenCredentialsAreValid_Test()
    {
        var request = new LoginRequest { Email = "test01@hotmail.com", Password = "Test01" };
        var json = JsonConvert.SerializeObject(request);

        var response = await PostAsync("api/v1/auth/login", json);

        response.StatusCode.ShouldBe(HttpStatusCode.OK);
    }

    [Fact]
    public async Task RegisterAsync_ShouldReturnCreated_WhenValidRequestIsGiven_Test()
    {
        var request = new RegisterDemand
        {
            FirstName = "John", LastName = "Doe", Currency = "EUR", TimeZone = "Europe/Brussels",
            Email = "johndoe@hotmail.com", ConfirmPassword = "Password123!", Password = "Password123!",
            Locale = "en-US"
        };

        var json = JsonConvert.SerializeObject(request);

        var correlationId = Guid.NewGuid().ToString();
        var response = await PostAsync("api/v1/auth/register", json, correlationId);
        response.StatusCode.ShouldBe(HttpStatusCode.Created);

        var message = await WriteDbContext.OutboxMessages.SingleOrDefaultAsync(el => el.CorrelationId == correlationId);
        message.Type.ShouldBe(typeof(RegisteredEvent).AssemblyQualifiedName);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("NvsWkemm*f16idqmJQ@ZNm*lSX5zKI6Ti7aFL0eDoIfNQiA2vkhQjEy3NV")]
    public async Task RegisterAsync_ShouldReturnBadRequestWhenFirstNameIsNotValid_Test(string value)
    {
        var request = new RegisterDemand
        {
            FirstName = value, LastName = "Integration", Currency = "EUR", TimeZone = "Europe/Brussels",
            Email = "test01@integration.com", ConfirmPassword = "password123", Password = "password123",
            Locale = "en-US"
        };

        var json = JsonConvert.SerializeObject(request);
        var response = await PostAsync("api/v1/auth/register", json);
        
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

        var failures = JsonConvert.DeserializeObject<List<Failure>>(await response.Content.ReadAsStringAsync());
        var failure = failures.First();
        
        failure.Code.ShouldBe(FailureConstant.User.FirstNameInvalid.Code);
        failure.Message.ShouldBe(FailureConstant.User.FirstNameInvalid.Message);
    }
    
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("I@A&N3V8xceDu5mrPHwETuWr!l!acd5V995Rx2@Cn!gDTmC1@JvWM4U7SobW7e0aWjprlpbmTe3ytT!Yt#g0G@4xJOn80jm$^%X123456")]
    public async Task RegisterAsync_ShouldReturnBadRequestWhenLastNameIsNotValid_Test(string value)
    {
        var request = new RegisterDemand
        {
            FirstName = "John Doe", LastName = value, Currency = "EUR", TimeZone = "Europe/Brussels",
            Email = "test01@integration.com", ConfirmPassword = "password123", Password = "password123",
            Locale = "en-US"
        };

        var json = JsonConvert.SerializeObject(request);
        var response = await PostAsync("api/v1/auth/register", json);
        
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

        var failures = JsonConvert.DeserializeObject<List<Failure>>(await response.Content.ReadAsStringAsync());
        var failure = failures.First();
        
        failure.Code.ShouldBe(FailureConstant.User.LastNameInvalid.Code);
        failure.Message.ShouldBe(FailureConstant.User.LastNameInvalid.Message);
    }
    
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("I@A&N3V8xceDu5mrPHwETuWr!l!acd5V995Rx2@Cn!gDTmC1")]
    public async Task RegisterAsync_ShouldReturnBadRequestWhenEmailIsNotValid_Test(string value)
    {
        var request = new RegisterDemand
        {
            FirstName = "John", LastName = "Doe", Currency = "EUR", TimeZone = "Europe/Brussels",
            Email = value, ConfirmPassword = "password123", Password = "password123",
            Locale = "en-US"
        };

        var json = JsonConvert.SerializeObject(request);
        var response = await PostAsync("api/v1/auth/register", json);
        
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

        var failures = JsonConvert.DeserializeObject<List<Failure>>(await response.Content.ReadAsStringAsync());
        var failure = failures.First();
        
        failure.Code.ShouldBe(FailureConstant.User.EmailInvalid.Code);
        failure.Message.ShouldBe(FailureConstant.User.EmailInvalid.Message);
    }
    
    [Theory]
    [InlineData(null)]
    [InlineData("A non valid TimeZone")]
    public async Task RegisterAsync_ShouldReturnBadRequestWhenTimeZoneIsNotValid_Test(string value)
    {
        var request = new RegisterDemand
        {
            FirstName = "John", LastName = "Doe", Currency = "EUR", TimeZone = value,
            Email = "johndoe@testing.com", ConfirmPassword = "password123", Password = "password123",
            Locale = "en-US"
        };

        var json = JsonConvert.SerializeObject(request);
        var response = await PostAsync("api/v1/auth/register", json);
        
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

        var failures = JsonConvert.DeserializeObject<List<Failure>>(await response.Content.ReadAsStringAsync());
        var failure = failures.First();
        
        failure.Code.ShouldBe(FailureConstant.User.TimeZoneInvalid.Code);
        failure.Message.ShouldBe(FailureConstant.User.TimeZoneInvalid.Message);
    }
    
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("A non valid currency")]
    public async Task RegisterAsync_ShouldReturnBadRequestWhenCurrencyIsNotValid_Test(string value)
    {
        var request = new RegisterDemand
        {
            FirstName = "John", LastName = "Doe", Currency = value, TimeZone = "Europe/Brussels",
            Email = "johndoe@testing.com", ConfirmPassword = "password123", Password = "password123",
            Locale = "en-US"
        };

        var json = JsonConvert.SerializeObject(request);
        var response = await PostAsync("api/v1/auth/register", json);
        
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

        var failures = JsonConvert.DeserializeObject<List<Failure>>(await response.Content.ReadAsStringAsync());
        var failure = failures.First();
        
        failure.Code.ShouldBe(FailureConstant.User.CurrencyInvalid.Code);
        failure.Message.ShouldBe(FailureConstant.User.CurrencyInvalid.Message);
    }
    
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("A non valid Locale")]
    public async Task RegisterAsync_ShouldReturnBadRequestWhenLocaleIsNotValid_Test(string value)
    {
        var request = new RegisterDemand
        {
            FirstName = "John", LastName = "Doe", Currency = "EUR", TimeZone = "Europe/Brussels",
            Email = "johndoe@testing.com", ConfirmPassword = "password123", Password = "password123",
            Locale = value
        };

        var json = JsonConvert.SerializeObject(request);
        var response = await PostAsync("api/v1/auth/register", json);
        
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

        var failures = JsonConvert.DeserializeObject<List<Failure>>(await response.Content.ReadAsStringAsync());
        var failure = failures.First();
        
        failure.Code.ShouldBe(FailureConstant.User.LocaleInvalid.Code);
        failure.Message.ShouldBe(FailureConstant.User.LocaleInvalid.Message);
    }
    
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("A non valid password")]
    [InlineData("pwd")]
    [InlineData("password")]
    [InlineData("Password")]
    [InlineData("Password1")]
    [InlineData("Password$")]
    public async Task RegisterAsync_ShouldReturnBadRequestWhenPasswordIsNotValid_Test(string value)
    {
        var request = new RegisterDemand
        {
            FirstName = "John", LastName = "Doe", Currency = "EUR", TimeZone = "Europe/Brussels",
            Email = "johndoe@testing.com", ConfirmPassword = "password123", Password = value,
            Locale = "en-US"
        };

        var json = JsonConvert.SerializeObject(request);
        var response = await PostAsync("api/v1/auth/register", json);
        
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

        var failures = JsonConvert.DeserializeObject<List<Failure>>(await response.Content.ReadAsStringAsync());
        var failure = failures.First();
        
        failure.Code.ShouldBe(FailureConstant.User.PasswordInvalid.Code);
        failure.Message.ShouldBe(FailureConstant.User.PasswordInvalid.Message);
    }
    
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("A non valid password")]
    [InlineData("pwd")]
    [InlineData("password")]
    [InlineData("Password")]
    [InlineData("Password1")]
    [InlineData("Password$")]
    public async Task RegisterAsync_ShouldReturnBadRequestWhenConfirmPasswordIsNotValid_Test(string value)
    {
        var request = new RegisterDemand
        {
            FirstName = "John", LastName = "Doe", Currency = "EUR", TimeZone = "Europe/Brussels",
            Email = "johndoe@testing.com", ConfirmPassword = value, Password = "Password123!",
            Locale = "en-US"
        };

        var json = JsonConvert.SerializeObject(request);
        var response = await PostAsync("api/v1/auth/register", json);
        
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

        var failures = JsonConvert.DeserializeObject<List<Failure>>(await response.Content.ReadAsStringAsync());
        var failure = failures.First();
        
        failure.Code.ShouldBe(FailureConstant.User.ConfirmPasswordInvalid.Code);
        failure.Message.ShouldBe(FailureConstant.User.ConfirmPasswordInvalid.Message);
    }
    
    
    [Fact]
    public async Task GetProfileInformationAsync_ShouldReturnInformationBasedOnAccessToken_Test()
    {
        var response = await GetAuthenticatedAsync("api/v1/auth/profile");
        
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        var result = await response.Content.ReadAsStringAsync();
        var body = JsonConvert.DeserializeObject<ProfileInformationResult>(result);
        if (body == null) throw new ApplicationException("Could not read profile information");
        
        body.Name.ShouldBe("John Doe");
        body.Email.ShouldBe("johndoe@hotmail.com");
        body.Locale.ShouldBe("en-US");
        body.TimeZone.ShouldBe("Europe/Brussels");
        body.Currency.ShouldBe("EUR");
        body.Id.ShouldBe("123");
    }
}