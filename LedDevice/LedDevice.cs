using Magals.DevicesControl.SDKStandart;
using Magals.DevicesControl.SDKStandart.Attributes;
using Magals.DevicesControl.SDKStandart.Interfeces.GPIO;
using Magals.DevicesControl.SDKStandart.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Device.Gpio;
using System.Linq;

namespace LedDevice
{
  [GpioSettings]
  [Driver("Led")]
  public class LedDevice : ILed
  {
    private bool _isConnected;
    private int _pinLamp;
    private PinValue _startValue;

    public bool IsConnected => _isConnected;

    public bool EnableAutoScan => throw new NotImplementedException();

    public event EventHandler<LoggerEventArgs> LogMessage;

    private GpioSettingsAttribute gpioSettings;

    private GpioController gpioController;
    public LedDevice()
    {
      try
      {
        gpioController = new GpioController();
      }
      catch (Exception ex)
      {
        LogMessage?.Invoke(this, new LoggerEventArgs(LogLevel.Error, ex.ToString()));
      }
      gpioSettings = DeviceConfig.GetSettingsFromAttribute<GpioSettingsAttribute>(this);
    }

    [NotSupport]
    public void AutoDetectDevice()
    {
      throw new NotImplementedException();
    }

    public bool Connect()
    {
      LogMessage.Invoke(this, new LoggerEventArgs(LogLevel.Information, $"Connect"));
      try
      {
        if (gpioSettings.outputLow.Any())
        {
          _pinLamp = gpioSettings.outputLow.First();
          _startValue = PinValue.Low;
        }
        else if (gpioSettings.outputHigh.Any())
        {
          _pinLamp = gpioSettings.outputHigh.First();
          _startValue = PinValue.High;
        }

        if (_pinLamp != 0)
        {
          gpioController.OpenPin(_pinLamp, PinMode.Output, PinValue.Low);
          _isConnected = true;
        }
        else
        {
          _isConnected = false;
          LogMessage.Invoke(this, new LoggerEventArgs(LogLevel.Warning, $"Pin Led == 0"));
        }
      }
      catch (Exception ex)
      {
        LogMessage.Invoke(this, new LoggerEventArgs(LogLevel.Error, ex.ToString()));
        _isConnected = false;
      }

      return IsConnected;
    }

    [NotSupport]
    public object CustomMethod(object custom)
    {
      throw new NotImplementedException();
    }

    [NotSupport]
    public void Disconnect()
    {
      throw new NotImplementedException();
    }

    public void Dispose()
    {
      gpioController.Dispose();
    }

    [NotSupport]
    public string GetSerialDevice()
    {
      return this.GetType().Name;
    }

    [NotSupport]
    public Dictionary<string, object> GetStatuses()
    {
      throw new NotImplementedException();
    }

    public void OnOff(bool state)
    {
      gpioController.Write(_pinLamp, state);
    }
  }
}
