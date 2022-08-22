using Magals.DevicesControl.Core.Models;
using Magals.DevicesControl.Core;
using Microsoft.Extensions.Logging;
using static Magals.DevicesControl.Core.Models.DevicesConfigModel;
using Magals.DevicesControl.SDKStandart.Attributes;
using Magals.DevicesControl.SDKStandart.Interfeces;

namespace InternetThings.T0.Tests
{
	public class MainTemperatureMeterT0Tests_IInternetThing_Tcp
	{
		private IInternetThing? internetThing;

		public MainTemperatureMeterT0Tests_IInternetThing_Tcp()
		{
			var dcm = new SettingsDevices.Config
			{
				enable = true,
				name = "1_name_config",
				autoscan = false,
				type_connect = "tcp",
				config = new TcpSettingsAttribute("https://localhost", 49155)
			};

			InstanceLogicDevices _mainlogic = new();
			_mainlogic.CreateIntanceForConfig(typeof(MainTemperatureMeterT0), dcm);
			internetThing = _mainlogic.GetInstanceByNameConfig<IInternetThing>("T0", "1_name_config");
		}

		[Fact()]
		public void AutoDetectDeviceTest()
		{

		}

		[Fact()]
		public void ConnectTest()
		{
			internetThing?.Connect();
			internetThing?.IsConnected.ShouldBeTrue();
		}

		[Fact()]
		public void CustomMethodTest()
		{

		}

		[Fact()]
		public void DisconnectTest()
		{

		}

		[Fact()]
		public void DisposeTest()
		{

		}

		[Fact()]
		public void GetDataTest()
		{
			var result = internetThing?.GetData("ask") ?? new();
			((string)result).Length.ShouldBePositive();
			result.ShouldNotBeNull();
		}

		[Fact()]
		public void GetSerialDeviceTest()
		{
			var serial = internetThing?.GetSerialDevice();
			Guid.TryParse(serial, out var device).ShouldBeTrue();
		}

		[Fact()]
		public void GetStatusesTest()
		{

		}

		[Fact()]
		public void SendCommandTest()
		{

		}
	}
}