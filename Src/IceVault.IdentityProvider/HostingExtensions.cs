using IceVault.IdentityProvider.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace IceVault.IdentityProvider
{
    internal static class HostingExtensions
    {
        public static WebApplication ConfigureServices(this WebApplicationBuilder builder)
        {
            var migrationAssembly = typeof(HostingExtensions).Assembly.GetName().Name;
            var connectionString = builder.Configuration["Database:ConnectionString"];

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(connectionString, sql => sql.MigrationsAssembly(migrationAssembly));
            });

            builder.Services.AddIdentity<ApplicationUser, IdentityRole>().AddDefaultTokenProviders().AddEntityFrameworkStores<ApplicationDbContext>();

            builder.Services.AddIdentityServer(options => options.EmitStaticAudienceClaim = true)
                .AddConfigurationStore(options => options.ConfigureDbContext = b =>
                    b.UseSqlServer(connectionString, sql => sql.MigrationsAssembly(migrationAssembly)))
                .AddOperationalStore(options => options.ConfigureDbContext = b =>
                    b.UseSqlServer(connectionString, sql => sql.MigrationsAssembly(migrationAssembly)))
                .AddAspNetIdentity<ApplicationUser>()
                .AddProfileService<IceVaultProfileService>();

            return builder.Build();
        }

        public static WebApplication ConfigurePipeline(this WebApplication app)
        {
            app.UseSerilogRequestLogging();

            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                DatabaseSeeder.Seed(app);
            }

            app.UseIdentityServer();

            return app;
        }
    }
}