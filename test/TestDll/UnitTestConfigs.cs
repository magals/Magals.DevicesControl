namespace TestDll
{
	public class UnitTestConfigs
	{
		static ILogger<InstanceLogicDevices>? logger;
		public UnitTestConfigs()
		{
			ILoggerFactory loggerFactory =
										LoggerFactory.Create(builder =>
										{
											builder.SetMinimumLevel(LogLevel.Trace);
										});

			logger = loggerFactory.CreateLogger<InstanceLogicDevices>();
			Directory.CreateDirectory(@"temp");
		}

		[Fact]
		public void TestCreateDevicesConfig()
		{
			Configure _configure = new(logger, @"Resources\TemplateSerialportConfig.json");
			_configure.ParseConfig();

			_configure.DevicesConfigModel.defaultlist.Count.ShouldBe(1);
			_configure.DevicesConfigModel.settingsdevices.Count.ShouldBe(1);
			_configure.DevicesConfigModel.settingsdevices.First().configs.Count.ShouldBe(1);
		}

		[Fact]
		public void TestAddConfigSerialPortSettingsAndSave()
		{
			Configure _configure = new(logger, @"temp\default.json");

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
							key = "key1",
							value = "value1"
						}
					}
				}
			};
			_configure.AddConfig("temp_device", config);

			_configure.DevicesConfigModel.settingsdevices.Count.ShouldBe(1);
			_configure.DevicesConfigModel.settingsdevices.First().configs.Count.ShouldBe(1);
			_configure.DevicesConfigModel.settingsdevices.First().configs.First().Equals(config).ShouldBe(true);

			_configure.SaveConfigs();

			Configure _configureCheck = new(logger, @"temp\default.json");
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
		public void TestAdd_and_RemoveConfigFictionalTypeConnectSettingsAndSave()
		{
			Configure _configure = new(logger, @"temp\defaultFictionalTypeConnect.json");
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
							key = "key1",
							value = "value1"
						}
					}
				}
			};
			_configure.AddConfig("temp_device", config);

			_configure.DevicesConfigModel.settingsdevices.Count.ShouldBe(1);
			_configure.DevicesConfigModel.settingsdevices.First().configs.Count.ShouldBe(1);
			_configure.DevicesConfigModel.settingsdevices.First().configs.First().Equals(config).ShouldBe(true);

			_configure.SaveConfigs();

			Configure _configureCheck = new(logger, @"temp\defaultFictionalTypeConnect.json");
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

			_configureCheck.RemoveConfig("temp_device", "1_name_config");

			_configureCheck.DevicesConfigModel.settingsdevices.First().configs.Any().ShouldBeFalse();
		}

        [Fact]
        public void TestAdd_and_ParseString()
        {
            Configure _configure = new(logger, @"temp\defaultFictionalTypeConnect.json");
			var jsonconfig = """
			   {
			  "settingsdevices": [
			    {
			      "name": "temp_device",
			      "configs": [
			        {
			          "name": "1_name_config",
			          "enable": true,
			          "type_connect": "fictionaltypeconnect",
			          "config": {
			            "abstractfield": "RunTimeTest"
			          },
			          "protocol": "test",
			          "autoscan": false,
			          "description": "",
			          "customsettings": [
			            {
			              "key": "key1",
			              "value": "value1"
			            }
			          ]
			        }
			      ],
			      "enable": true
			    }
			  ]
			}
			""";

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
                            key = "key1",
                            value = "value1"
                        }
                    }
                }
            };

            _configure.SaveConfigs();

            Configure _configureCheck = new(logger, @"temp\defaultFictionalTypeConnect.json");
            _configureCheck.ParseConfig();

            var text = JsonSerializer.Serialize(config, new JsonSerializerOptions()
            {
                IgnoreReadOnlyProperties = true,
                WriteIndented = true,

            });

            Configure _configureCheck2 = new(logger, "");
            _configureCheck2.ParseConfig(jsonconfig);

            var text2 = JsonSerializer.Serialize(_configureCheck2.DevicesConfigModel.settingsdevices.First().configs.First(), new JsonSerializerOptions()
            {
                IgnoreReadOnlyProperties = true,
                WriteIndented = true,

            });
            string.Equals(text, text2).ShouldBe(true);
        }

        [Fact]
		public void TestCreateDefaultConfigsByLoadedDriver()
		{
			InstanceLogicDevices _mainlogic = new(logger: logger,
																										 pathconfig: @"temp\TestCreateConfigFileWithConfigs.json");

			_mainlogic.LoadAllDrivers();
			_mainlogic.CreateInstance();

			Configure _configureCheck = new(logger, @"temp\TestCreateConfigFileWithConfigs.json");
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