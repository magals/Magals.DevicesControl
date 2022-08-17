using Magals.DevicesControl.SDKStandart.Interfeces;

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
			MainLogicDevices _mainlogic = new MainLogicDevices(logger: logger,
				pathconfig: "Resources/FictionalTypeConnect.json");
			_mainlogic.ParseConfig();
			_mainlogic.LoadAllDrivers();
			_mainlogic.CreateIntance();

			_mainlogic.Instances.Count().ShouldBe(1);

			var device = _mainlogic.GetInstanceByNameConfig<IFictionalDevice>("FictionalDevice", "1_name_config");

			
			device.ShouldNotBeNull();

			var serial = device.GetSerialDevice();
			Guid.TryParse(serial, out Guid result).ShouldBe(true);

			var settings = DeviceConfig.GetSettingsFromAttribute<FictionalTypeConnectSettingsAttribute>(device);
			string.Equals(settings.abstractfield, "RunTimeTest").ShouldBe(true);
		}
		
		[Fact]
		public void TestCreateDefaultInstance()
		{
			MainLogicDevices _mainlogic = new MainLogicDevices(logger: logger,
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

		
	}
}
