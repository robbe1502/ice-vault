using IceVault.Application;
using IceVault.Common.Settings;
using IceVault.Infrastructure;
using IceVault.Infrastructure.BackgroundJobs;
using IceVault.Persistence.Read;
using IceVault.Persistence.Write;
using IceVault.Presentation.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Quartz;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers()
    .AddApplicationPart(typeof(AuthenticationController).Assembly);

// Dependencies
builder.Services.AddApplication();
builder.Services.AddInfrastructure();
builder.Services.AddWritePersistence();
builder.Services.AddReadPersistence(builder.Configuration);

builder.Services.AddHttpClient();
builder.Services.AddHttpContextAccessor();

builder.Services.AddAutoMapper(typeof(AuthenticationController).Assembly);

// Logging
builder.Logging.AddSeq(builder.Configuration["Logging:Seq:Url"]);

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Background Jobs
builder.Services.AddQuartz(config =>
{
    var processOutboxMessageKey = new JobKey(nameof(ProcessOutboxMessagesJob));
    config.UseMicrosoftDependencyInjectionJobFactory();

    config.AddJob<ProcessOutboxMessagesJob>(processOutboxMessageKey).AddTrigger(trigger =>
        trigger.ForJob(processOutboxMessageKey)
            .WithSimpleSchedule(schedule => schedule.WithIntervalInSeconds(30).RepeatForever()));
});

builder.Services.AddQuartzHostedService();


// Settings
builder.Services.Configure<IdentitySetting>(builder.Configuration.GetSection("Identity"));
builder.Services.Configure<PersistenceSetting>(builder.Configuration.GetSection("Persistence"));


// Authentication & Authorization
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(
    JwtBearerDefaults.AuthenticationScheme,
    options =>
    {
        options.Authority = builder.Configuration["Identity:Authority"];
        options.RequireHttpsMetadata = bool.Parse(builder.Configuration["Identity:RequireHttps"]);
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateAudience = false
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

app.MapControllers().RequireAuthorization("ApiScope");
app.Run();
