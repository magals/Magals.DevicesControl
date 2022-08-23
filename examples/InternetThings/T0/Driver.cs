using InternetThings.Temperature;
using Magals.DevicesControl.SDKStandart.Attributes;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.IO.Ports;
using System.Linq;
using System.Net.Http;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace InternetThings.T0
{
	internal class Driver
	{
		private SerialPort? _comPort;
		private static HttpClient httpClient = new HttpClient();
		private Client? client;

		public List<byte> _bufferData = new List<byte>();

		public Driver(SerialPortSettingsAttribute settingsSerialPort)
		{
			_comPort = new SerialPort();
			_comPort.Parity = settingsSerialPort.parity;
			_comPort.PortName = settingsSerialPort.portname;
			_comPort.BaudRate = settingsSerialPort.baudrate;
			_comPort.DataBits = settingsSerialPort.dataBits;
			_comPort.StopBits = settingsSerialPort.stopbits;
		}

		public Driver(TcpSettingsAttribute settingsTCP)
		{
			client = new Client($"{settingsTCP.ip}:{settingsTCP.port}/", httpClient);
		}

		public bool Connect()
		{
			var sds = client?.WeatherforecastAsync().Result;
			return sds.Any();
		}

		public void SendCommand(string message)
		{

		}

		public string GetData(string message)
		{
			var sds = client?.WeatherforecastAsync().Result;
			return "[" + string.Join(", ", sds.Select(i => i.ToString()).ToArray()) + "]";
		}

		public string GetSerial()
		{
			return Guid.NewGuid().ToString();
		}

		public void CloseConnect()
		{
			httpClient.Dispose();
		}

	}
}
