using Magals.DevicesControl.DbContext.DbContexts;
using Magals.DevicesControl.DbContext.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.OpenApi;
using WebApplicationGenMigrations;
using Magals.DevicesControl.DbContext.Entities;
using Microsoft.AspNetCore.Builder;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<SettingsDevicesRepository>();
builder.Services.AddScoped<ConfigRepository>();
builder.Services.AddScoped<CustomsettingsRepository>();
builder.Services.AddDbContext<SettingsDevicesDbContext>(options => 
{ 
    options.UseNpgsql("Server=localhost;Port=5432;Userid=postgres;Password=mysecretpassword;Pooling=false;MinPoolSize=1;MaxPoolSize=20;Timeout=15;SslMode=Disable;Database=SD",
         npgsqlOptionsAction: sqlOptions =>
         {
             sqlOptions.MigrationsAssembly("WebApplicationGenMigrations");
         }); 
});
var app = builder.Build().MigrateDatabase<SettingsDevicesDbContext>(); ;

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/", async (HttpContext context) =>
{
    context.Response.Redirect("/swagger");
    await context.Response.CompleteAsync();
});

app.MapPost("/SettingsDevicesRepository", async (SettingsDevicesRepository sdr, SettingsDevicesEntity sde) =>
{
     await sdr.Add(sde);
}).WithOpenApi(); ;
//dotnet ef migrations add InitialMigrations --project ..\Magals.DevicesControl.DbContext\Magals.DevicesControl.DbContext.csproj --startup-project WebApplicationGenMigrations.csproj -c SettingsDevicesDbContext_PostgreSQL -o ..\Magals.DevicesControl.DbContext\DbContexts\PostgreSQL\Migrations
app.Run();
