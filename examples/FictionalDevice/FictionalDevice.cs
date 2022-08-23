using Magals.DevicesControl.SDKStandart;
using Magals.DevicesControl.SDKStandart.Attributes;
using Magals.DevicesControl.SDKStandart.Interfeces;
using Magals.DevicesControl.SDKStandart.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace FictionalDevice
{
#pragma warning disable 67
	[FictionalTypeConnectSettings("testing")]
	[Driver("FictionalDevice")]
	public class FictionalDevice : IFictionalDevice
	{
		private bool _connected = false;
		public bool IsConnected => _connected;

		private bool _enableAutoScan = false;
		public bool EnableAutoScan => _enableAutoScan;

		public event EventHandler<LoggerEventArgs> LogMessage;

		private FictionalTypeConnectSettingsAttribute fictionalTypeConnectSettings;
		public FictionalDevice()
		{
			fictionalTypeConnectSettings = DeviceConfig.GetSettingsFromAttribute<FictionalTypeConnectSettingsAttribute>(this);
		}

		public void AutoDetectDevice()
		{
			throw new NotImplementedException();
		}

		public bool Connect()
		{
			LogMessage.Invoke(this, new LoggerEventArgs(LogLevel.Information, $"Connect"));
			_connected = true;
			return _connected;
		}

		public object CustomMethod(object custom)
		{
			throw new NotImplementedException();
		}

		public void Disconnect()
		{
			_connected = false;
		}

		public void Dispose()
		{
			GC.SuppressFinalize(this);
		}

		public string GetSerialDevice()
		{
			return Guid.NewGuid().ToString();
		}

		public Dictionary<string, object> GetStatuses()
		{
			return new Dictionary<string, object>()
			{
				{ "state", true }
			};
		}

		public void TestMethod(string temp)
		{
			throw new NotImplementedException();
		}
	}
}
