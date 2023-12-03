using Magals.DevicesControl.SDKStandart;
using Magals.DevicesControl.SDKStandart.Attributes;
using Magals.DevicesControl.SDKStandart.Interfeces.GPIO;
using Magals.DevicesControl.SDKStandart.Models;
using System.Device.Gpio;
using Microsoft.Extensions.Logging;

namespace RGBLed;

[GpioSettings]
[Driver("RGBLed_L298N")]
public class RGBLed_L298N : IRGBLed
{
    private bool _isConnected;
    private int _pinLedRed;
    private PinValue _startValueLedRed;

    private int _pinLedGreen;
    private PinValue _startValueLedGreen;

    private int _pinLedBlue;
    private PinValue _startValueBlue;

    public RGBLed_L298N()
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

    public bool IsConnected => _isConnected;

    public bool EnableAutoScan => throw new NotImplementedException();

    public event EventHandler<LoggerEventArgs> LogMessage;

    private GpioSettingsAttribute gpioSettings;

    private GpioController gpioController;

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
            if(gpioSettings.outputLow.Count() != 3 &&
               gpioSettings.outputHigh.Count() != 3)
            {
                throw new ArgumentException("outputLow or outputHigh less 3");
            }

            

            if (gpioSettings.outputLow.Any())
            {
                _pinLedRed = gpioSettings.outputLow[0];
                _pinLedGreen = gpioSettings.outputLow[1];
                _pinLedBlue = gpioSettings.outputLow[2];

                _startValueLedRed = PinValue.Low;
                _startValueLedGreen = PinValue.Low;
                _startValueBlue = PinValue.Low;
            }
            else if (gpioSettings.outputHigh.Any())
            {
                _pinLedRed = gpioSettings.outputHigh[0];
                _pinLedGreen = gpioSettings.outputHigh[1];
                _pinLedBlue = gpioSettings.outputHigh[2];

                _startValueLedRed = PinValue.High;
                _startValueLedGreen = PinValue.High;
                _startValueBlue = PinValue.High;
            }

            gpioController.OpenPin(_pinLedRed,      PinMode.Output, _startValueLedRed);
            gpioController.OpenPin(_pinLedGreen,    PinMode.Output, _startValueLedGreen);
            gpioController.OpenPin(_pinLedBlue,     PinMode.Output, _startValueBlue);
            _isConnected = true;

            LogMessage.Invoke(this, new LoggerEventArgs(LogLevel.Information, $"OpenPins Red:{_pinLedRed}, Green:{_pinLedGreen}, Blue:{_pinLedBlue}"));
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
        
    }

    public void Dispose()
    {
        gpioController.Dispose();
    }

    [NotSupport]
    public string GetSerialDevice()
    {
        throw new NotImplementedException();
    }

    [NotSupport]
    public Dictionary<string, object> GetStatuses()
    {
        throw new NotImplementedException();
    }

    public void OnOff(bool red, bool green, bool blue)
    {
        gpioController.Write(_pinLedRed,    red     == true ? PinValue.Low : PinValue.High);
        gpioController.Write(_pinLedGreen,  green   == true ? PinValue.Low : PinValue.High);
        gpioController.Write(_pinLedBlue,   blue    == true ? PinValue.Low : PinValue.High);
    }
}
