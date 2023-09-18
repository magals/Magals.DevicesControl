using Magals.DevicesControl.SDKStandart;
using Magals.DevicesControl.SDKStandart.Attributes;
using Magals.DevicesControl.SDKStandart.Interfeces;
using Magals.DevicesControl.SDKStandart.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace CameraDetectCarNumber_UNV
{
    [TcpSettings]
    [Driver("Camera_UNV")]
    public class MainCameraDetectCarNumber : ICameraDetectCarNumber
    {
        private readonly TcpSettingsAttribute _tcpSettings;

        public bool IsConnected => isConnected;
        bool isConnected = true;
        private bool _disposedValue;
        public bool EnableAutoScan => false;

        public event Action<string> DetectCarNumber;
        public event EventHandler<LoggerEventArgs> LogMessage;
        private Driver _driver;

        public MainCameraDetectCarNumber()
        {
            try
            {
                _tcpSettings = DeviceConfig.GetSettingsFromAttribute<TcpSettingsAttribute>(this);
                 if (!_tcpSettings.FlagDefault)
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

        [NotSupport]
        public void AutoDetectDevice()
        {
            throw new NotImplementedException();
        }

        public bool Connect()
        {
            _driver.StartTcpListener();
            _driver.DetectCarNumber += _driver_DetectCarNumber;
            return IsConnected;
        }

        private void _driver_DetectCarNumber(string obj)
        {
            DetectCarNumber?.Invoke(obj);
        }

        [NotSupport]
        public object CustomMethod(object custom)
        {
            throw new NotImplementedException();
        }

        public void Disconnect()
        {
            _driver.StopTcpListener();
            _driver.DetectCarNumber -= _driver_DetectCarNumber;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        // Protected implementation of Dispose pattern.
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    _driver = null;
                }

                _disposedValue = true;
            }
        }

        public string GetSerialDevice()
        {
            throw new NotImplementedException();
        }

        public Dictionary<string, object> GetStatuses()
        {
            throw new NotImplementedException();
        }
    }
}
