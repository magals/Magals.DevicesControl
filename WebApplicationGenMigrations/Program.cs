using Magals.DevicesControl.DbContext.DbContexts;
using Magals.DevicesControl.DbContext.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.OpenApi;
using WebApplicationGenMigrations;
using Magals.DevicesControl.DbContext.Entities;
using Microsoft.AspNetCore.Builder;
using Magals.DevicesControl.DbContext.DTOs;
using System.ComponentModel.DataAnnotations;
using Magals.DevicesControl.Core;
using static Magals.DevicesControl.Core.Models.DevicesConfigModel;
using static Magals.DevicesControl.Core.Models.DevicesConfigModel.SettingsDevices;
using static System.Net.Mime.MediaTypeNames;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Net.Http.Json;
using System.Runtime.Intrinsics.X86;
using Magals.DevicesControl.DbContext;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<SettingsDevicesRepository>();
builder.Services.AddScoped<ConfigRepository>();
builder.Services.AddScoped<CustomsettingsRepository>();


builder.Services.AddSingleton<InstanceLogicDevices>();


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
{/*
     await sdr.Add(new SettingsDevicesEntity
     {
         Name = sde.name,
         Enable = sde.enable,
         Configs = new List<ConfigsEntity>
         {
             new ConfigsEntity
             {
                 Name = sde.configs.First().name,
                 Enable = sde.configs.First().enable,
                 Description = sde.configs.First().description,
                 Autoscan = sde.configs.First().autoscan,
                 Protocol = sde.configs.First().protocol,
                 Type_Connect = sde.configs.First().type_connect,
                 Config = JsonSerializer.Serialize(sde.configs.First().config),
                 //SettingsDevicesName = sde.Name
             }
         }
     });*/
    await sdr.Add(Converts.ConvertDTOtoEntity<SettingsDevicesEntity>(sde));
}).WithOpenApi();

app.MapPost("/ConfigRepository", async (ConfigRepository cr,  string Namedevice, ConfigsDTO cdto) =>
{/*
    await cr.Add(Namedevice, new ConfigsEntity
    {
        Autoscan = cdto.autoscan,
        Protocol = cdto.protocol,
        Name = cdto.name,
        Enable = cdto.enable,
        Config = JsonSerializer.Serialize(cdto.config),
        Type_Connect = cdto.type_connect,
        Description = cdto.description,

    });*/
    await cr.Add(Namedevice, Converts.ConvertDTOtoEntity<ConfigsEntity>(cdto));
}).WithOpenApi();
app.MapGet("/SettingsDevicesRepository/GetAllSettings", async(SettingsDevicesRepository sdr) =>
{
    var tempresult =  await sdr.GetAllSettings();
    List<SettingsDevicesDTO> result = new List<SettingsDevicesDTO>();

    foreach (var item in tempresult)
    {
        result.Add(new SettingsDevicesDTO
        {
            name = item.Name,
            enable = item.Enable,
            configs = new List<ConfigsDTO>()
        });

        foreach (var config in item.Configs)
        {
            result.Last().configs.Add(new ConfigsDTO
            {
                name = config.Name,
                enable = config.Enable,
                autoscan = config.Autoscan,
                description = config.Description,
                type_connect = config.Type_Connect,
                protocol = config.Protocol,
                config = JsonSerializer.Deserialize<JsonElement>(config.Config),
            });
        }
       
    }
    return result;
});

app.MapGet("/CreateInstance", async (InstanceLogicDevices ild,SettingsDevicesRepository sdr) =>
{
    var settings = await sdr.GetAllSettings();
    List<SettingsDevicesDTO> result = new List<SettingsDevicesDTO>();

    foreach (var item in settings)
    {
        result.Add(new SettingsDevicesDTO
        {
            name = item.Name,
            enable = item.Enable,
            configs = new List<ConfigsDTO>()
        });

        foreach (var config in item.Configs)
        {
            result.Last().configs.Add(new ConfigsDTO
            {
                name = config.Name,
                enable = config.Enable,
                autoscan = config.Autoscan,
                description = config.Description,
                type_connect = config.Type_Connect,
                protocol = config.Protocol,
                config = JsonSerializer.Deserialize<JsonElement>(config.Config),
               
            });
        }
    }

    var  text = JsonSerializer.Serialize(result, new JsonSerializerOptions()
    {
        IgnoreReadOnlyProperties = true,
        WriteIndented = true,
        
    } );
    ild.Configure.ParseConfig("{\"settingsdevices\":" +text +"}");
    ild.LoadAllDrivers();
    ild.CreateInstance();
});
//dotnet ef migrations add InitialMigrations --project ..\Magals.DevicesControl.DbContext\Magals.DevicesControl.DbContext.csproj --startup-project WebApplicationGenMigrations.csproj -c SettingsDevicesDbContext_PostgreSQL -o ..\Magals.DevicesControl.DbContext\DbContexts\PostgreSQL\Migrations
app.Run();
