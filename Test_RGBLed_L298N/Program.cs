// See https://aka.ms/new-console-template for more information
using Magals.DevicesControl.Core;
using Magals.DevicesControl.Core.Models;
using Magals.DevicesControl.SDKStandart.Attributes;
using Magals.DevicesControl.SDKStandart.Interfeces;
using Magals.DevicesControl.SDKStandart.Interfeces.GPIO;
using Microsoft.Extensions.Logging;
using RGBLed;
using static Magals.DevicesControl.Core.Models.DevicesConfigModel;

Console.WriteLine("Hello, World!");
ILogger<RGBLed_L298N>? logger;
IRGBLed? rGBLed;
RGBLed_L298N mcdcn = new RGBLed_L298N();

ILoggerFactory loggerFactory =
                              LoggerFactory.Create(builder =>
                              {
                                  builder.SetMinimumLevel(LogLevel.Trace);
                              });

logger = loggerFactory.CreateLogger<RGBLed_L298N>();

var dcm = new SettingsDevices.Config
{
    enable = true,
    name = "1_name_config",
    autoscan = false,
    type_connect = "gpio",
    config = new GpioSettingsAttribute(new int[] {16,20,21}, new int[] { }, new int[] { }, new int[] { }, new int[] { })
};

var dcma = new DevicesConfigModel
{
    settingsdevices = new List<SettingsDevices>
                {
                    new SettingsDevices
                    {
                        name = "RGBLed_L298N",
                        enable = true,
                        configs = new()
                        {
                            dcm
                        }
                    }
                }
};

InstanceLogicDevices _mainlogic = new(pathconfig: @"RGBLed_L298N.json");
_mainlogic.Configure.DevicesConfigModel = dcma;

_mainlogic.CreateInstanceForConfig(typeof(RGBLed_L298N), dcm);
rGBLed = _mainlogic.GetInstanceByNameConfig<IRGBLed>("RGBLed_L298N", "1_name_config");
rGBLed.LogMessage += RGBLed_LogMessage;

void RGBLed_LogMessage(object? sender, Magals.DevicesControl.SDKStandart.Models.LoggerEventArgs e)
{
    Console.WriteLine(e.message);
}

rGBLed.Connect();


while (true)
{
    Console.WriteLine("RED");
    rGBLed.OnOff(true, false, false);
    Thread.Sleep(1000);

    Console.WriteLine("GREEN");
    rGBLed.OnOff(false, true, false);
    Thread.Sleep(1000);

    Console.WriteLine("BLUE");
    rGBLed.OnOff(false, false, true);
    Thread.Sleep(1000);

    Console.WriteLine("RED+GREEN");
    rGBLed.OnOff(true, true, false);
    Thread.Sleep(1000);

    Console.WriteLine("GREEN+BLUE");
    rGBLed.OnOff(false, true, true);
    Thread.Sleep(1000);

    Console.WriteLine("RED+BLUE");
    rGBLed.OnOff(true, false, true);
    Thread.Sleep(1000);

    Console.WriteLine("WHITE");
    rGBLed.OnOff(true, true, true);
    Thread.Sleep(1000);

    Console.WriteLine("OFF");
    rGBLed.OnOff(false, false, false);
    Thread.Sleep(1000);
}