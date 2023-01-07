using System.Reflection;
using FluentValidation;
using IceVault.Application;
using IceVault.Application.Authentication.Login;
using IceVault.Common.Settings;
using IceVault.Infrastructure;
using IceVault.Persistence.Read;
using IceVault.Persistence.Write;
using IceVault.Presentation.Authentication;
using IceVault.Presentation.Middleware.ExceptionHandling;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using NodaTime;
using NodaTime.Serialization.SystemTextJson;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddJsonOptions(options => options.JsonSerializerOptions.ConfigureForNodaTime(DateTimeZoneProviders.Tzdb))
    .AddApplicationPart(typeof(AuthenticationController).Assembly);

// Dependencies
builder.Services.AddApplication();
builder.Services.AddInfrastructure();
builder.Services.AddWritePersistence();
builder.Services.AddReadPersistence();

builder.Services.AddHttpClient();
builder.Services.AddHttpContextAccessor();

builder.Services.AddAutoMapper(typeof(AuthenticationController).Assembly);
builder.Services.AddValidatorsFromAssemblies(new List<Assembly> { typeof(LoginQuery).Assembly });

// Logging
builder.Logging.AddSeq(builder.Configuration["Logging:Seq:Url"]);

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Settings
builder.Services.Configure<IdentitySetting>(builder.Configuration.GetSection("Identity"));
builder.Services.Configure<PersistenceSetting>(builder.Configuration.GetSection("Persistence"));
builder.Services.Configure<MailSetting>(builder.Configuration.GetSection("Mail"));

// Authentication & Authorization
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(
    JwtBearerDefaults.AuthenticationScheme,
    options =>
    {
        options.Authority = builder.Configuration["Identity:Authority"];
        options.RequireHttpsMetadata = bool.Parse(builder.Configuration["Identity:RequireHttps"]);
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuer = true,
            ClockSkew = TimeSpan.Zero
        };
    });

builder.Services.AddAuthorization(options => options.AddPolicy("ApiScope", policy =>
{
    policy.RequireAuthenticatedUser();
    policy.RequireClaim("scope", "ice-vault-web-api");
}));

// Starting the app
var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.MapControllers().RequireAuthorization("ApiScope");
app.Run();

public partial class WebApiProgram {}