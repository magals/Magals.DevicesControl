using Magals.DevicesControl.DbContext.DbContexts;
using Magals.DevicesControl.DbContext.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.OpenApi;
using WebApplicationGenMigrations;
using Magals.DevicesControl.DbContext.Entities;
using Microsoft.AspNetCore.Builder;
using Magals.DevicesControl.DbContext.DTOs;
using System.ComponentModel.DataAnnotations;

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

app.MapPost("/SettingsDevicesRepository", async (SettingsDevicesRepository sdr, SettingsDevicesDTO sde) =>
{
     await sdr.Add(new SettingsDevicesEntity
     {
         Name = sde.Name,
         Enable = sde.Enable,
         Configs = new List<ConfigsEntity>
         {
             new ConfigsEntity
             {
                 Name = sde.Configs.First().Name,
                 Enable = sde.Configs.First().Enable,
                 Description = sde.Configs.First().Description,
                 Autoscan = sde.Configs.First().Autoscan,
                 Protocol = sde.Configs.First().Protocol,
                 Type_Connect = sde.Configs.First().Type_Connect,
                 Config = sde.Configs.First().Config,
                 //SettingsDevicesName = sde.Name
             }
         }
     });
}).WithOpenApi();

app.MapPost("/ConfigRepository", async (ConfigRepository cr,  string Namedevice, ConfigsDTO cdto) =>
{
    await cr.Add(Namedevice, new ConfigsEntity
    {
        Autoscan = cdto.Autoscan,
        Protocol = cdto.Protocol,
        Name = cdto.Name,
        Enable = cdto.Enable,
        Config = cdto.Config,
        Type_Connect = cdto.Type_Connect,
        Description = cdto.Description,

    });
}).WithOpenApi();
app.MapGet("/SettingsDevicesRepository/GetAllSettings", async(SettingsDevicesRepository sdr) =>
{
    var tempresult =  await sdr.GetAllSettings();
    List<SettingsDevicesDTO> result = new List<SettingsDevicesDTO>();

    foreach (var item in tempresult)
    {
        result.Add(new SettingsDevicesDTO
        {
            Name = item.Name,
            Enable = item.Enable,
            Configs = new List<ConfigsDTO>()
        });

        foreach (var config in item.Configs)
        {
            result.Last().Configs.Add(new ConfigsDTO
            {
                Name = config.Name,
                Enable = config.Enable,
                Autoscan = config.Autoscan,
                Description = config.Description,
                Type_Connect = config.Type_Connect,
                Protocol = config.Protocol,
                Config = config.Config,
            });
        }
       
    }
    return result;
});
//dotnet ef migrations add InitialMigrations --project ..\Magals.DevicesControl.DbContext\Magals.DevicesControl.DbContext.csproj --startup-project WebApplicationGenMigrations.csproj -c SettingsDevicesDbContext_PostgreSQL -o ..\Magals.DevicesControl.DbContext\DbContexts\PostgreSQL\Migrations
app.Run();
