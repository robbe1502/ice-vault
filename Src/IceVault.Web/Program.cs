using Microsoft.AspNetCore.SpaServices.AngularCli;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddCors();
builder.Services.AddControllers();
builder.Services.AddSpaStaticFiles(config => config.RootPath = "Client/dist");

var app = builder.Build();

app.UseSpaStaticFiles();
app.UseSpa(config =>
{
    config.Options.SourcePath = "Client";
    if (builder.Environment.IsDevelopment())
    {
        config.UseAngularCliServer("start");
    }
});


app.UseCors(config => config.WithOrigins(builder.Configuration["AllowedOrigins"]));
app.UseAuthorization();
app.MapControllers();

app.Run();
