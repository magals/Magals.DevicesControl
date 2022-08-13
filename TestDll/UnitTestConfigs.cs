


using Magals.DevicesControl.Core.Models;
using Magals.DevicesControl.SDKStandart.Attributes;
using System.Text.Json;
using static Magals.DevicesControl.Core.Models.DevicesConfigModel;

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
											builder.AddFile(options =>
											{
												options.FileName = "diagnostics-"; // The log file prefixes
												options.LogDirectory = "Logs"; // The directory to write the logs
												options.FileSizeLimit = 20 * 1024 * 1024; // The maximum log file size (20MB here)
												options.FilesPerPeriodicityLimit = 200; // When maximum file size is reached, create a new file, up to a limit of 200 files per periodicity
												options.Extension = "log"; // The log file extension
												options.Periodicity = PeriodicityOptions.Daily; // Roll log files hourly instead of daily.
											});

										});

			logger = loggerFactory.CreateLogger<UnitTestConfigs>();
			Directory.CreateDirectory(@"temp");
		}

		[Fact]
		public void TestCreateDevicesConfig()
		{
			Configure _configure = new Configure(logger);
			var fileStream = new FileStream(@"Resources\TemplateWithTwoConfigsDevices.json", FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None);
			_configure.ParseConfig(fileStream);

			_configure.DevicesConfigModel.defaultlist.Count().ShouldBe(1);
			_configure.DevicesConfigModel.settingsdevices.Count().ShouldBe(1);
			_configure.DevicesConfigModel.settingsdevices.First().configs.Count().ShouldBe(1);
		}

		[Fact]
		public void TestAddConfigSerialPortSettingsAndSave()
		{
			Configure _configure = new Configure(logger);

			var fileStream = new FileStream(@"temp\default.json", FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None);
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

			_configure.SaveConfigs(fileStream);

			Configure _configureCheck = new Configure(logger);
			fileStream.Position = 0;
			_configureCheck.ParseConfig(fileStream);

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
			Configure _configure = new Configure(logger);

			var fileStream = new FileStream(@"temp\defaultFictionalTypeConnect.json", FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None);
			var config = new SettingsDevices.Config
			{
				enable = true,
				name = "1_name_config",
				autoscan = false,
				description = String.Empty,
				protocol = "test",
				type_connect = "fictionaltypeconnect",
				config = new FictionalTypeConnectSettingsAttribute("RunTumeTest"),
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

			_configure.SaveConfigs(fileStream);

			Configure _configureCheck = new Configure(logger);
			fileStream.Position = 0;
			_configureCheck.ParseConfig(fileStream);

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
	}
}