using Magals.DevicesControl.SDKStandart.Interfeces.GPIO;
using Microsoft.Extensions.Logging;

namespace TestDll
{
  public class UnitTestLedInstance
  {
    static ILogger<InstanceLogicDevices>? logger;
    public UnitTestLedInstance()
    {
      ILoggerFactory loggerFactory =
                    LoggerFactory.Create(builder =>
                    {
                      builder.SetMinimumLevel(LogLevel.Trace);
                    });

      logger = loggerFactory.CreateLogger<InstanceLogicDevices>();
    }

    [Fact]
    public void TestCreateSimpleInstance()
    {
      InstanceLogicDevices _mainlogic = new(logger: logger,
        pathconfig: "Resources/LedDeviceConfig.json");
      _mainlogic.ParseConfig();
      _mainlogic.LoadAllDrivers();
      _mainlogic.CreateInstance();

      _mainlogic.Instances.Count.ShouldBe(1);

      var device = _mainlogic.GetInstanceByNameConfig<ILed>("Led", "1_name_config");
      device.ShouldNotBeNull();

      var serial = device.GetSerialDevice();
      string.Equals(serial, "LedDevice_1_name_config").ShouldBe(true);

    }

    [Fact]
    public void TestCreateDefaultInstance()
    {
      InstanceLogicDevices _mainlogic = new(logger: logger,
                                                         pathconfig: @"Resources\LedDeviceConfig.json");
      _mainlogic.ParseConfig();
      _mainlogic.LoadAllDrivers();
      _mainlogic.CreateInstance();

      _mainlogic.Instances.Count.ShouldBe(1);
      var settings = DeviceConfig.GetSettingsFromAttribute<DriverAttribute>(_mainlogic.Instances.First());
      string.Equals(settings.name, "Led").ShouldBe(true);

      var config = _mainlogic.GetConfigByInstance(_mainlogic.Instances.First());

      string.Equals(config.name, "1_name_config").ShouldBe(true);
    }

    [Fact]
    public void TestGetDefaultFictionalDevice()
    {
      InstanceLogicDevices _mainlogic = new(logger: logger,
                                                     pathconfig: "Resources/LedDeviceConfig.json");
      _mainlogic.ParseConfig();
      _mainlogic.LoadAllDrivers();
      _mainlogic.CreateInstance();

      var ifd = _mainlogic.GetNameDefaultInstance<ILed>();
      object.Equals(ifd, _mainlogic.GetInstanceByNameConfig<ILed>("Led", "1_name_config")).ShouldBe(true);
    }

    [Fact]
    public void TestChangeConfigByInstance()
    {
      var dcm = new DevicesConfigModel
      {
        settingsdevices = new List<SettingsDevices>
        {
          new SettingsDevices
          {
            name = "Led",
            enable = true,
            configs = new()
            {
              new SettingsDevices.Config
              {
                enable = true,
                name = "1_name_config",
                autoscan = false,
                description = String.Empty,
                protocol = "test",
                type_connect = "fictionaltypeconnect",
                //config = new GpioSettingsAttribute("RunTimeTest"),
                //customsettings = new List<SettingsDevices.Config.Customsettings>
                //{
                //  {
                //    new SettingsDevices.Config.Customsettings
                //    {
                //      key = "key1",
                //      value = "value1"
                //    }
                //  }
                //}
              }
            }
          }
        }
      };

      InstanceLogicDevices _mainlogic = new(logger: logger,
                                                            pathconfig: @"temp\TestChangeConfigByInstance.json");

      _mainlogic.Configure.DevicesConfigModel = dcm;
      _mainlogic.LoadAllDrivers();
      _mainlogic.CreateInstance();

      var instance = _mainlogic.GetInstanceByNameConfig<ILed>("FictionalDevice", "1_name_config");

      var changeconfig = new SettingsDevices.Config
      {
        enable = true,
        name = "1_name_config",
        autoscan = false,
        description = String.Empty,
        protocol = "test",
        type_connect = "fictionaltypeconnect",
        //config = new GpioSettingsAttribute("RunTimeChange"),
        customsettings = new()
      };

      var atr = DeviceConfig.GetSettingsFromAttribute<GpioSettingsAttribute>(instance);
      //string.Equals(atr.abstractfield, "RunTimeTest").ShouldBeTrue();

      _mainlogic.SetConfigByInstance(instance, changeconfig);

      instance = _mainlogic.GetInstanceByNameConfig<ILed>("FictionalDevice", "1_name_config");
      var atr2 = DeviceConfig.GetSettingsFromAttribute<GpioSettingsAttribute>(instance);

      //string.Equals(atr2.abstractfield, "RunTimeChange").ShouldBeTrue();

      File.Delete(@"temp\TestChangeConfigByInstance.json");
    }

  }
}
