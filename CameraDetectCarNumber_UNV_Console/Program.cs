using CameraDetectCarNumber_UNV;
using Magals.DevicesControl.Core;
using Magals.DevicesControl.Core.Models;
using Magals.DevicesControl.SDKStandart.Attributes;
using Magals.DevicesControl.SDKStandart.Interfeces;
using Microsoft.Extensions.Logging;
using static Magals.DevicesControl.Core.Models.DevicesConfigModel;

ILogger<MainCameraDetectCarNumber_UNV>? logger;
ICameraDetectCarNumber? cameraDetectCarNumber;
MainCameraDetectCarNumber_UNV mcdcn = new MainCameraDetectCarNumber_UNV();

ILoggerFactory loggerFactory =
                              LoggerFactory.Create(builder =>
                              {
                                  builder.SetMinimumLevel(LogLevel.Trace);
                              });

logger = loggerFactory.CreateLogger<MainCameraDetectCarNumber_UNV>();

var dcm = new SettingsDevices.Config
{
    enable = true,
    name = "1_name_config",
    autoscan = false,
    type_connect = "tcp",
    config = new TcpSettingsAttribute("192.168.88.206", 3333)
};

var dcma = new DevicesConfigModel
{
    settingsdevices = new List<SettingsDevices>
                {
                    new SettingsDevices
                    {
                        name = "MainCameraDetectCarNumber_UNV",
                        enable = true,
                        configs = new()
                        {
                            dcm
                        }
                    }
                }
};

InstanceLogicDevices _mainlogic = new(pathconfig: @"MainCameraDetectCarNumber_UNV.json");
_mainlogic.Configure.DevicesConfigModel = dcma;
_mainlogic.Configure.SaveConfigs();

_mainlogic.CreateInstanceForConfig(typeof(MainCameraDetectCarNumber_UNV), dcm);
cameraDetectCarNumber = _mainlogic.GetInstanceByNameConfig<ICameraDetectCarNumber>("Camera_UNV", "1_name_config");
cameraDetectCarNumber.LogMessage += CameraDetectCarNumber_LogMessage;
cameraDetectCarNumber.Connect();



void CameraDetectCarNumber_LogMessage(object? sender, Magals.DevicesControl.SDKStandart.Models.LoggerEventArgs e)
{
    Console.WriteLine($"{e.time}   {e.message}");
}

cameraDetectCarNumber.DetectCarNumber += CameraDetectCarNumber_DetectCarNumber;

Console.ReadLine();

void CameraDetectCarNumber_DetectCarNumber(string obj)
{
    Console.WriteLine(obj);
}