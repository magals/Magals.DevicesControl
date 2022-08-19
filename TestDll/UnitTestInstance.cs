using Magals.DevicesControl.Core.Models;
using Magals.DevicesControl.SDKStandart.Interfeces;
using Microsoft.Diagnostics.Runtime;
using System.Diagnostics;
using System.Dynamic;

namespace TestDll
{
	public class UnitTestInstance
	{
		static ILogger? logger;
		public UnitTestInstance()
		{
			ILoggerFactory loggerFactory =
										LoggerFactory.Create(builder =>
										{
											builder.SetMinimumLevel(LogLevel.Trace);
										});

			logger = loggerFactory.CreateLogger<UnitTestConfigs>();
		}

		[Fact]
		public void TestCreateSimpleInstance()
		{
			MainLogicDevices _mainlogic = new(logger: logger,
				pathconfig: "Resources/FictionalTypeConnect.json");
			_mainlogic.ParseConfig();
			_mainlogic.LoadAllDrivers();
			_mainlogic.CreateIntance();

			_mainlogic.Instances.Count.ShouldBe(1);

			var device = _mainlogic.GetInstanceByNameConfig<IFictionalDevice>("FictionalDevice", "1_name_config");
			device.ShouldNotBeNull();

			var serial = device.GetSerialDevice();
			Guid.TryParse(serial, out _).ShouldBe(true);

			var settings = DeviceConfig.GetSettingsFromAttribute<FictionalTypeConnectSettingsAttribute>(device);
			string.Equals(settings.abstractfield, "RunTimeTest").ShouldBe(true);
		}

		[Fact]
		public void TestCreateDefaultInstance()
		{
			MainLogicDevices _mainlogic = new(logger: logger,
																												 pathconfig: @"Resources\FictionalTypeConnect.json");
			_mainlogic.ParseConfig();
			_mainlogic.LoadAllDrivers();
			_mainlogic.CreateIntance();

			_mainlogic.Instances.Count.ShouldBe(1);
			var settings = DeviceConfig.GetSettingsFromAttribute<DriverAttribute>(_mainlogic.Instances.First());
			string.Equals(settings.name, "FictionalDevice").ShouldBe(true);

			var config = _mainlogic.GetConfigByInstance(_mainlogic.Instances.First());

			string.Equals(config.name, "1_name_config").ShouldBe(true);
		}

		[Fact]
		public void TestGetDefaultFictionalDevice()
		{
			MainLogicDevices _mainlogic = new(logger: logger,
																										 pathconfig: "Resources/FictionalTypeConnect.json");
			_mainlogic.ParseConfig();
			_mainlogic.LoadAllDrivers();
			_mainlogic.CreateIntance();

			var ifd = _mainlogic.GetNameDefaultInstance<IFictionalDevice>();
			object.Equals(ifd, _mainlogic.GetInstanceByNameConfig<IFictionalDevice>("FictionalDevice", "1_name_config")).ShouldBe(true);
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
						name = "FictionalDevice",
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
								config = new FictionalTypeConnectSettingsAttribute("RunTimeTest"),
								customsettings = new List<SettingsDevices.Config.Customsettings>
								{
									{
										new SettingsDevices.Config.Customsettings
										{
											Key = "key1",
											Value = "value1"
										}
									}
								}
							}
						}
					}
				}
			};

			MainLogicDevices _mainlogic = new(logger: logger,
																										 pathconfig: @"temp\TestChangeConfigByInstance.json");
			
			_mainlogic.Configure.DevicesConfigModel = dcm;
			_mainlogic.LoadAllDrivers();
			_mainlogic.CreateIntance();

			var instance = _mainlogic.GetInstanceByNameConfig<IFictionalDevice>("FictionalDevice", "1_name_config");

			var changeconfig = new SettingsDevices.Config
			{
				enable = true,
				name = "1_name_config",
				autoscan = false,
				description = String.Empty,
				protocol = "test",
				type_connect = "fictionaltypeconnect",
				config = new FictionalTypeConnectSettingsAttribute("RunTimeChange"),
				customsettings = new ()
			};

			var atr = DeviceConfig.GetSettingsFromAttribute<FictionalTypeConnectSettingsAttribute>(instance);
			string.Equals(atr.abstractfield, "RunTimeTest").ShouldBeTrue();

			_mainlogic.SetConfigByInstance(instance, changeconfig);

			instance = _mainlogic.GetInstanceByNameConfig<IFictionalDevice>("FictionalDevice", "1_name_config");
			var atr2 = DeviceConfig.GetSettingsFromAttribute<FictionalTypeConnectSettingsAttribute>(instance);

			string.Equals(atr2.abstractfield, "RunTimeChange").ShouldBeTrue();

			File.Delete(@"temp\TestChangeConfigByInstance.json");
		}
	}
}
