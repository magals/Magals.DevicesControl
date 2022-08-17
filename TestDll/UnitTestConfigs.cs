using System.IO;
using static Magals.DevicesControl.Core.Models.DevicesConfigModel.SettingsDevices;

namespace TestDll
{
	public class UnitTestConfigs
	{
		static ILogger? logger;
		public UnitTestConfigs()
		{
			ILoggerFactory loggerFactory =
										LoggerFactory.Create(builder =>
										{
											builder.SetMinimumLevel(LogLevel.Trace);
										});

			logger = loggerFactory.CreateLogger<UnitTestConfigs>();
			Directory.CreateDirectory(@"temp");
		}

		[Fact]
		public void TestCreateDevicesConfig()
		{
			Configure _configure = new Configure(logger, @"Resources\TemplateWithTwoConfigsDevices.json");
			_configure.ParseConfig();

			_configure.DevicesConfigModel.defaultlist.Count().ShouldBe(1);
			_configure.DevicesConfigModel.settingsdevices.Count().ShouldBe(1);
			_configure.DevicesConfigModel.settingsdevices.First().configs.Count().ShouldBe(1);
		}

		[Fact]
		public void TestAddConfigSerialPortSettingsAndSave()
		{
			Configure _configure = new Configure(logger, @"temp\default.json");

		//	var fileStream = new FileStream(@"temp\default.json", FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None);
			var config = new SettingsDevices.Config
			{
				enable = true,
				name = "1_name_config",
				autoscan = false,
				description = String.Empty,
				protocol = "test",
				type_connect = "serialport",
				config = new SerialPortSettingsAttribute(),
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
			};
			_configure.AddConfig("temp_device", config);

			_configure.DevicesConfigModel.settingsdevices.Count().ShouldBe(1);
			_configure.DevicesConfigModel.settingsdevices.First().configs.Count().ShouldBe(1);
			_configure.DevicesConfigModel.settingsdevices.First().configs.First().Equals(config).ShouldBe(true);

			_configure.SaveConfigs();

			Configure _configureCheck = new Configure(logger, @"temp\default.json");
			_configureCheck.ParseConfig();

			var text = JsonSerializer.Serialize(config, new JsonSerializerOptions()
			{
				IgnoreReadOnlyProperties = true,
				WriteIndented = true,

			});
			var text2 = JsonSerializer.Serialize(_configureCheck.DevicesConfigModel.settingsdevices.First().configs.First(), new JsonSerializerOptions()
			{
				IgnoreReadOnlyProperties = true,
				WriteIndented = true,

			});
			string.Equals(text, text2) .ShouldBe(true);
		}

		[Fact]
		public void TestAddConfigFictionalTypeConnectSettingsAndSave()
		{
			Configure _configure = new Configure(logger, @"temp\defaultFictionalTypeConnect.json");
			var config = new SettingsDevices.Config
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
			};
			_configure.AddConfig("temp_device", config);

			_configure.DevicesConfigModel.settingsdevices.Count().ShouldBe(1);
			_configure.DevicesConfigModel.settingsdevices.First().configs.Count().ShouldBe(1);
			_configure.DevicesConfigModel.settingsdevices.First().configs.First().Equals(config).ShouldBe(true);

			_configure.SaveConfigs();

			Configure _configureCheck = new Configure(logger, @"temp\defaultFictionalTypeConnect.json");
			_configureCheck.ParseConfig();

			var text = JsonSerializer.Serialize(config, new JsonSerializerOptions()
			{
				IgnoreReadOnlyProperties = true,
				WriteIndented = true,

			});
			var text2 = JsonSerializer.Serialize(_configureCheck.DevicesConfigModel.settingsdevices.First().configs.First(), new JsonSerializerOptions()
			{
				IgnoreReadOnlyProperties = true,
				WriteIndented = true,

			});
			string.Equals(text, text2).ShouldBe(true);
		}

		[Fact]
		public void TestCreateDefaultConfigsByLoadedDriver()
		{
			MainLogicDevices _mainlogic = new MainLogicDevices(logger: logger,
																										     pathconfig: @"temp\TestCreateConfigFileWithConfigs.json");

			_mainlogic.LoadAllDrivers();
			_mainlogic.CreateIntance();

			Configure _configureCheck = new Configure(logger, @"temp\TestCreateConfigFileWithConfigs.json");
			_configureCheck.ParseConfig();

			var text = JsonSerializer.Serialize(_mainlogic.Configure.GetConfigsForDevice("FictionalDevice")?.configs.First(), new JsonSerializerOptions()
			{
				IgnoreReadOnlyProperties = true,
				WriteIndented = true
			});
			var text2 = JsonSerializer.Serialize(_configureCheck.DevicesConfigModel.settingsdevices.First().configs.First(), new JsonSerializerOptions()
			{
				IgnoreReadOnlyProperties = true,
				WriteIndented = true
			});
			string.Equals(text, text2).ShouldBe(true);

			File.Delete(@"temp\TestCreateConfigFileWithConfigs.json");
		}
	}
}