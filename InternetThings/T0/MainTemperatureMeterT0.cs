using Magals.DevicesControl.SDKStandart;
using Magals.DevicesControl.SDKStandart.Attributes;
using Magals.DevicesControl.SDKStandart.Interfeces;
using Magals.DevicesControl.SDKStandart.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace InternetThings.T0
{
	[SerialPortSettings]
	[TcpSettings]
	[Driver("T0")]
	public class MainTemperatureMeterT0 : IInternetThing
	{
		private readonly TcpSettingsAttribute? _tcpSettings;
		private readonly SerialPortSettingsAttribute? _serialPortSettings;
		private readonly Driver? _driver;

		bool isConnected = false;
		public MainTemperatureMeterT0()
		{
			try
			{
				_tcpSettings = DeviceConfig.GetSettingsFromAttribute<TcpSettingsAttribute>(this);
				_serialPortSettings = DeviceConfig.GetSettingsFromAttribute<SerialPortSettingsAttribute>(this);
				
				if (!_serialPortSettings.FlagDefault)
				{
					LogMessage?.Invoke(this, new LoggerEventArgs(LogLevel.Information, $"settings serialport setup. port:{_serialPortSettings.portname}" +
																																																					 $"baudrate:{_serialPortSettings.baudrate}"));
					_driver = new Driver(_serialPortSettings);
				}
				else if (!_tcpSettings.FlagDefault)
				{
					LogMessage?.Invoke(this, new LoggerEventArgs(LogLevel.Information, $"settings tcp setup. ip:{_tcpSettings.ip} port:{_tcpSettings.port}"));
					_driver = new Driver(_tcpSettings);
				}
			}
			catch (Exception ex)
			{
				LogMessage?.Invoke(this, new LoggerEventArgs(LogLevel.Error, ex.ToString()));
				isConnected = false;
			}
		}

		public bool IsConnected => isConnected;

		public bool EnableAutoScan => false;

		public event EventHandler<LoggerEventArgs>? LogMessage;

		public void AutoDetectDevice()
		{
			throw new NotImplementedException();
		}

		public bool Connect()
		{
			isConnected = _driver?.Connect() ?? false;
			return IsConnected;
		}

		public object CustomMethod(object custom)
		{
			throw new NotImplementedException();
		}

		public void Disconnect()
		{
			_driver?.CloseConnect();
		}

		public void Dispose()
		{
			throw new NotImplementedException();
		}

		public object GetData(string command)
		{
			return _driver?.GetData(command) ?? string.Empty;
		}

		public string GetSerialDevice()
		{
			return _driver?.GetSerial() ?? string.Empty;
		}

		public Dictionary<string, object> GetStatuses()
		{
			throw new NotImplementedException();
		}

		public void SendCommand(string command)
		{
			throw new NotImplementedException();
		}
	}
}
