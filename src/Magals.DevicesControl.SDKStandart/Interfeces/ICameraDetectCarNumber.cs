using System;

namespace Magals.DevicesControl.SDKStandart.Interfeces
{
    public interface ICameraDetectCarNumber : IDevice
    {
        event Action<string> DetectCarNumber;
    }
}
